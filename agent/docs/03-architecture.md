# 03 — Arquitectura del Sistema UCC_SIGA

## 1. Visión General y Estilo Arquitectónico

La solución **UCC_SIGA** adopta los principios de **Clean Architecture** (Arquitectura Limpia) combinada con **CQRS** (Command Query Responsibility Segregation) y organización interna por **Vertical Slices** (rebanadas verticales por caso de uso).

### Principios Rectores:
1. **Independencia del Dominio (Inversión de Dependencias):** La capa `SIGA.Domain` es el núcleo soberano del sistema. No referencia a ninguna librería de infraestructura ni persistencia. Declara los contratos de persistencia que necesita a través de interfaces de repositorio (`SIGA.Domain/Interfaces/Repositories/`).
2. **Desacoplamiento de Persistencia:** `SIGA.Application` no conoce `ApplicationDbContext` ni Entity Framework Core; interactúa con el dominio a través de agregados y repositorios, y delega consultas complejas de lectura a `QueryServices`.
3. **Mediador Propio sin MediatR:** Se implementa un despachador desacoplado (`UseCaseDispatcher`) montado nativamente sobre el contenedor de dependencias (`IServiceProvider`) de ASP.NET Core, evitando dependencias externas pesadas.
4. **Mapeo en Tiempo de Compilación:** Se utiliza exclusivamente **Riok.Mapperly** mediante generadores de código fuente (*source generators*), eliminando sobrecargas en runtime y garantizando chequeos de tipos estrictos.

---

## 2. Diagrama de Contenedores y Contexto (C4)

```mermaid
graph TD
    subgraph Usuarios
        Aspirante["Aspirante / Comunidad<br>(Navegador Web)"]
        Admin["Personal Administrativo<br>(Navegador Web)"]
        Validador["Tercero Validador<br>(Lector QR / Móvil)"]
    end

    subgraph Aplicaciones_Frontend
        PortalAstro["Portal Público Web<br>[Astro Framework]<br>(Catálogo y Preinscripción)"]
        BlazorWebApp["SIGA.WebApp<br>[Blazor InteractiveServer - MudBlazor]<br>(Panel Administrativo)"]
    end

    subgraph Backend_Core
        WebApi["SIGA.WebApi<br>[ASP.NET Core REST API - .NET 10]<br>(Controladores, JWT, Swagger, RFC 7807)"]
        Application["SIGA.Application<br>(Casos de Uso, CQRS, Validaciones, Mapperly)"]
        Domain["SIGA.Domain<br>(Entidades, Agregados, Invariantes, Interfaces Repo)"]
        Persistence["SIGA.Persistence<br>(EF Core 10, Repositorios, IdentityDbContext)"]
        Infrastructure["SIGA.Infrastructure<br>(Scriban, PuppeteerSharp, CsvHelper, SHA-256)"]
    end

    subgraph Almacenamiento
        Postgres[("Base de Datos PostgreSQL 16<br>[db-siga / Esquema siga]<br>(Tablas de Negocio + Identity)")]
    end

    Aspirante -->|Navega cursos y preinscribe| PortalAstro
    Admin -->|Gestiona propuestas y alumnos| BlazorWebApp
    Validador -->|Escanea QR para verificar diploma| WebApi

    PortalAstro -->|HTTP REST / JSON| WebApi
    BlazorWebApp -->|HTTP REST / JSON con Bearer JWT| WebApi

    WebApi --> Application
    Application --> Domain
    Persistence -.->|Implementa interfaces de| Domain
    Infrastructure -.->|Implementa interfaces de| Application
    Persistence --> Postgres
```

---

## 3. Estructura y Responsabilidad de Capas .NET

```mermaid
graph LR
    subgraph Solucion_SIGA
        WebApi["1. SIGA.WebApi<br>(Presentation Layer)"]
        WebApp["2. SIGA.WebApp<br>(UI Client Layer)"]
        Shared["3. SIGA.Shared<br>(Contracts / DTOs)"]
        App["4. SIGA.Application<br>(Use Cases / CQRS)"]
        Dom["5. SIGA.Domain<br>(Core Domain / Repositories)"]
        Pers["6. SIGA.Persistence<br>(EF Core / Npgsql)"]
        Infra["7. SIGA.Infrastructure<br>(PDF / Templates / CSV)"]
    end

    WebApi --> App
    WebApi --> Pers
    WebApi --> Infra
    WebApi --> Shared
    
    WebApp --> Shared

    App --> Dom
    Pers --> Dom
    Pers --> App
    Infra --> App
```

| Capa / Proyecto | Dependencias | Responsabilidad Técnica |
|---|---|---|
| **`SIGA.Domain`** | Ninguna (cero referencias a capas externas) | Entidades de negocio (`Propuesta`, `PropuestaVersion`, `Alumno`, `Inscripcion`, `Certificado`), Enums, Excepciones (`DomainException`) y contratos de persistencia (`IRepository<T>`, `IUnitOfWork`). |
| **`SIGA.Application`** | `SIGA.Domain` | Casos de uso estructurados por Vertical Slices (`Features/`), Commands, Queries, Dispatcher nativo, validadores FluentValidation, mapeos Riok.Mapperly e interfaces de Query Services. |
| **`SIGA.Persistence`** | `SIGA.Application`, `SIGA.Domain` | `ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>`, mapeos de entidades EF Core (`IEntityTypeConfiguration<T>`), implementación de repositorios y migraciones PostgreSQL. |
| **`SIGA.Infrastructure`** | `SIGA.Application` | Implementación de servicios técnicos auxiliares: `CertificadoPdfService` (Scriban + PuppeteerSharp), `ReporteService` (CsvHelper) y generador criptográfico SHA-256. |
| **`SIGA.WebApi`** | `SIGA.Application`, `SIGA.Persistence`, `SIGA.Infrastructure`, `SIGA.Shared` | Host REST API, Composition Root (inyección de dependencias), autenticación JWT con ASP.NET Core Identity, middleware de manejo global de excepciones (ProblemDetails) y OpenAPI/Swagger. |
| **`SIGA.WebApp`** | `SIGA.Shared` | Frontend administrativo en Blazor Server con MudBlazor 8.*. Consume exclusivamente los endpoints de `SIGA.WebApi` mediante `HttpClient`. |
| **`SIGA.Shared`** | Ninguna | Contratos DTO compartidos y modelos de respuesta comunes. |

---

## 4. Diagrama Entidad-Relación Consolidado (ERD)

```mermaid
erDiagram
    unidades ||--o{ propuestas : "pertenece a"
    unidades ||--o{ autoridades : "posee"
    unidades ||--o{ usuarios_unidades : "asigna acceso a"
    tipos_propuesta ||--o{ propuestas : "clasifica"
    periodos_lectivos ||--o{ propuestas_versiones : "dictada en"
    periodos_lectivos ||--o{ autoridades : "vigente en"
    modalidades ||--o{ propuestas_versiones : "modalidad de"

    propuestas ||--|| propuestas_web : "seccion web 1:1"
    propuestas ||--o{ temarios : "plan de estudios"
    propuestas ||--o{ propuestas_versiones : "instancia cohortes"

    propuestas_versiones ||--o{ propuestas_versiones_docentes : "cuerpo docente"
    docentes ||--o{ propuestas_versiones_docentes : "asignado a"

    propuestas_versiones ||--o{ preinscripciones : "recibe solicitudes"
    propuestas_versiones ||--o{ inscripciones : "matricula alumnos"

    tipos_documento ||--o{ alumnos : "identifica con"
    tipos_documento ||--o{ preinscripciones : "identifica con"
    tipos_documento ||--o{ docentes : "identifica con"

    alumno_estados ||--o{ alumnos : "estado de"
    preinscripcion_estados ||--o{ preinscripciones : "estado de"
    inscripcion_estados ||--o{ inscripciones : "estado de"
    propuesta_estados ||--o{ propuestas_versiones : "estado de"
    certificado_estados ||--o{ certificados : "estado de"

    alumnos ||--o{ preinscripciones : "asocia si aprueba"
    alumnos ||--o{ inscripciones : "cursa"

    preinscripciones ||--o| inscripciones : "origina"
    inscripciones ||--o{ certificados : "emite historial"

    aspnet_users ||--o{ usuarios_unidades : "tiene unidades"
    aspnet_roles ||--o{ aspnet_role_claims : "permisos claims"

    propuestas {
        int id PK
        uuid uuid
        int unidad_id FK
        int tipo_propuesta_id FK
        string titulo
        string descripcion
        bool activo
    }

    propuestas_versiones {
        int id PK
        uuid uuid
        int propuesta_id FK
        int periodo_lectivo_id FK
        int modalidad_id FK
        int estado_propuesta_id FK
        date fecha_inicio
        date fecha_fin
        int maximo_alumnos
        decimal importe_base
        int cuotas
        string concepto_pago
        bool permite_inscripciones_web
        datetime creado_en
    }

    alumnos {
        int id PK
        uuid uuid
        int tipo_documento_id FK
        string numero_documento
        string apellido
        string nombre
        date fecha_nacimiento
        string email
        int alumno_estado_id FK
        bool activo
    }

    preinscripciones {
        int id PK
        uuid uuid
        int propuesta_version_id FK
        int modalidad_id FK
        int tipo_documento_id FK
        string numero_documento
        string apellido
        string nombre
        string email
        int estado_preinscripcion_id FK
        int alumno_id FK "nullable"
        datetime creado_en
    }

    inscripciones {
        int id PK
        uuid uuid
        int alumno_id FK
        int propuesta_version_id FK
        int preinscripcion_id FK
        int inscripcion_estado_id FK
        datetime fecha_inscripcion
        bool es_baja
    }

    certificados {
        int id PK
        uuid uuid
        string token
        int inscripcion_id FK
        int certificado_estado_id FK
        int version
        bool es_version_actual
        string hash_seguridad "SHA-256"
        string titulo_certificado
        int horas_certificadas
        date fecha_emision
        string url_verificacion
        string ruta_almacenamiento_pdf
    }

    autoridades {
        int id PK
        int unidad_id FK
        int periodo_lectivo_id FK
        string nombre
        string apellido
        string cargo
        string firma_img
        bool firma_certificado
        int orden_firma
        bool activo
    }
```

---

## 5. Flujos de Datos Críticos

### 5.1 Flujo de Admisión y Matriculación Atómica

```mermaid
sequenceDiagram
    autonumber
    actor Aspirante
    participant PortalWeb as Portal Web Público (Astro)
    participant Api as WebApi / Application
    participant Db as Base de Datos (PostgreSQL)
    actor Admin as Administrativo de Unidad

    Aspirante->>PortalWeb: Completa formulario de preinscripción
    PortalWeb->>Api: POST /api/preinscripciones
    Api->>Db: Inserta Preinscripcion (Estado: Pendiente)
    Db-->>Api: Confirma inserción con UUID
    Api-->>PortalWeb: Retorna confirmación al aspirante

    Note over Admin, Api: Revisión administrativa y comprobación de pago
    Admin->>Api: POST /api/preinscripciones/{id}/aprobar
    activate Api
    Note over Api, Db: INICIO DE TRANSACCIÓN ATÓMICA
    Api->>Db: Busca Alumno por TipoDocumento y NumeroDocumento
    alt Alumno no existe
        Api->>Db: Inserta nuevo Alumno (Estado: Activo)
    else Alumno existe
        Api->>Db: Valida que Alumno no esté Suspendido/Bloqueado y reutiliza AlumnoId
    end
    Api->>Db: Actualiza Preinscripcion (Estado: Aprobada, AlumnoId vinculado)
    Api->>Db: Inserta nueva Inscripcion (Estado: Activa, PropuestaVersionId)
    Note over Api, Db: COMMIT TRANSACCIÓN ATÓMICA
    deactivate Api
    Api-->>Admin: Retorna éxito con InscripcionId generada
```

### 5.2 Ciclo de Vida y Transición de Estados de PropuestaVersion

```mermaid
stateDiagram-v2
    [*] --> Borrador: Creación inicial por administrativo
    
    state Borrador {
        [*] --> EdicionTotal: Modificación de fechas, cupos, aranceles
        EdicionTotal --> EliminacionFisica: Permitida si no tiene inscripciones
    }

    Borrador --> Publicada: Acción "Publicar" (Valida lugar, modalidad y aranceles)

    state Publicada {
        [*] --> AdmisionWeb: Permite preinscripciones y visibilidad web
        AdmisionWeb --> EdicionRestringida: Solo campos no estructurales
    }

    Publicada --> Archivada: Acción "Archivar" (Fin de cohorte o ciclo lectivo)

    state Archivada {
        [*] --> SoloLectura: Inscripciones cerradas y web deshabilitada
        SoloLectura --> EmisionCertificados: Habilita certificación de inscripciones finalizadas
    }

    Archivada --> [*]
```

### 5.3 Flujo de Emisión y Versionado de Certificados

```mermaid
sequenceDiagram
    autonumber
    actor Admin as Administrativo
    participant Api as WebApi / Application
    participant Infra as Infrastructure (Scriban + Puppeteer)
    participant Db as PostgreSQL

    Admin->>Api: POST /api/certificados/generar { inscripcionId }
    Api->>Db: Valida Inscripcion (Finalizada) y Alumno (Activo)
    Api->>Db: Recupera Autoridades de la Unidad para el PeriodoLectivo (FirmaCertificado = true)
    
    Api->>Api: Genera Token único y calcula Hash criptográfico SHA-256
    Api->>Infra: Renderiza template certificado.html con Scriban (datos + firmas)
    Api->>Infra: Convierte HTML a PDF con PuppeteerSharp incorporando código QR
    Infra-->>Api: Retorna PDF generado en almacenamiento / MemoryStream
    
    Api->>Db: Inserta Certificado (Version: 1, EsVersionActual: true, Estado: Generado)
    Api-->>Admin: Retorna Certificado listo para descarga o aprobación

    Note over Admin, Api: Caso de corrección textual posterior
    Admin->>Api: PUT /api/certificados/{id} { nuevoTexto }
    Api->>Db: Actualiza versión anterior (EsVersionActual: false)
    Api->>Api: Recalcula nuevo Hash SHA-256
    Api->>Db: Inserta nuevo registro Certificado (Version: 2, EsVersionActual: true)
    Api-->>Admin: Retorna nueva versión manteniendo histórico legal
```

---

## 6. Esquema de Seguridad y Doble Dimensión

```mermaid
flowchart TD
    Req[Petición HTTP entrante con Bearer JWT] --> AuthFilter[Middleware de Autenticación JWT]
    AuthFilter -->|Token Inválido / Expirado| R401[401 Unauthorized]
    AuthFilter -->|Token Válido| ClaimsExtractor[Extraer Claims de Usuario: Permisos y Unidades]
    
    ClaimsExtractor --> PermisoCheck{¿El usuario tiene el<br>Claim de Operación?<br>ej: PROPUESTA.PUBLICAR}
    PermisoCheck -->|No| R403P[403 Forbidden: Permiso insuficiente]
    
    PermisoCheck -->|Sí| ResourceLookup[Obtener recurso de negocio y su UnidadId]
    ResourceLookup --> UnidadCheck{¿El recurso pertenece a<br>una de las Unidades Académicas<br>asignadas al usuario?}
    
    UnidadCheck -->|No| R403U[403 Forbidden: Unidad académica no autorizada]
    UnidadCheck -->|Sí| HandlerExecution[Ejecutar Caso de Uso en Application]
```
