# 05 — Estándares y Convenciones del Proyecto UCC_SIGA

Este documento establece las directrices normativas de desarrollo, estructura de directorios, estilo de código, manejo de errores, pruebas y documentación para todos los desarrolladores y agentes que intervengan en la solución.

---

## 1. Estructura Canónica de Carpetas y Proyectos

```text
UCC_SIGA/
├── .vscode/                                 # Configuraciones del IDE (launch, tasks)
├── agent/                                   # Documentación, contexto y herramientas de IA
│   └── docs/                                # Suite de especificación viva (00 a 07)
├── doc/                                     # Documentación funcional histórica y requerimientos
├── src/                                     # Código fuente del sistema
│   ├── SIGA.Domain/                         # Núcleo soberano (Cero dependencias)
│   │   ├── Common/                          # EntityBase (setters públicos), ValueObjects
│   │   ├── Entities/
│   │   │   ├── Catalog/
│   │   │   │   ├── Dynamic/                 # Unidad, PeriodoLectivo, Modalidad, TipoPropuesta
│   │   │   │   └── Static/                  # AlumnoEstado, CertificadoEstado, etc.
│   │   │   └── Core/                        # Propuesta, PropuestaWeb, PropuestaVersion,
│   │   │                                    # Alumno, Preinscripcion, Inscripcion, Certificado,
│   │   │                                    # Autoridad, Docente, Temario
│   │   ├── Enums/                           # Enumeraciones de dominio tipadas
│   │   ├── Exceptions/                      # DomainException
│   │   └── Interfaces/
│   │       └── Repositories/                # IPropuestaRepository, IInscripcionRepository,
│   │                                        # ICertificadoRepository, IUnitOfWork
│   ├── SIGA.Application/                    # Casos de uso (Depende solo de SIGA.Domain)
│   │   ├── Common/
│   │   │   ├── Dispatcher/                  # UseCaseDispatcher, IUseCase, IUseCaseHandler
│   │   │   ├── Exceptions/                  # AppException, NotFoundException, ValidationException
│   │   │   ├── Interfaces/Queries/          # IPropuestaQueryService, IInscripcionQueryService
│   │   │   └── Models/                      # AppResult<T>, PaginatedList<T>
│   │   ├── Features/                        # Vertical Slices por Agregado / Feature
│   │   │   ├── Propuestas/
│   │   │   │   ├── CrearPropuestaBorrador/  # Command, Handler, Validator, Mapperly, DTOs
│   │   │   │   ├── CrearVersionCohorte/
│   │   │   │   ├── PublicarVersion/
│   │   │   │   └── ArchivarVersion/
│   │   │   ├── Preinscripciones/
│   │   │   │   └── CrearPreinscripcion/
│   │   │   ├── Inscripciones/
│   │   │   │   ├── AprobarPreinscripcion/
│   │   │   │   └── FinalizarInscripcion/
│   │   │   └── Certificados/
│   │   │       ├── GenerarCertificado/
│   │   │       └── RectificarCertificado/
│   │   └── Interfaces/                      # Contratos externos: ICertificadoPdfService, IReporteService
│   ├── SIGA.Persistence/                    # Acceso a datos (Depende de Domain y Application)
│   │   ├── Configurations/                  # EntityTypeConfigurations (Namespace corregido)
│   │   ├── Context/                         # ApplicationDbContext : IdentityDbContext<User, Role, int>
│   │   ├── Migrations/                      # Migraciones EF Core PostgreSQL
│   │   ├── Queries/                         # Implementaciones de QueryServices (Dapper/EF NoTracking)
│   │   └── Repositories/                    # Implementaciones concretas de IRepository y UnitOfWork
│   ├── SIGA.Infrastructure/                 # Servicios técnicos externos (Depende de Application)
│   │   ├── Certificados/
│   │   │   ├── Services/                    # CertificadoPdfService (Scriban + PuppeteerSharp)
│   │   │   └── Template/                    # certificado.html, CSS, fuentes Montserrat y logos
│   │   ├── Cryptography/                    # Generador de Hash SHA-256 para códigos QR
│   │   └── Reporting/                       # ReporteService (CsvHelper)
│   ├── SIGA.Shared/                         # Contratos y DTOs comunes entre WebApi y WebApp
│   ├── SIGA.WebApi/                         # Host REST API (Composition Root)
│   │   ├── Controllers/                     # Controladores REST delgados
│   │   ├── Extensions/                      # Configuración de JWT, Swagger, CORS, DependencyInjection
│   │   └── Middleware/                      # ExceptionMiddleware (RFC 7807 ProblemDetails)
│   └── SIGA.WebApp/                         # Frontend administrativo Blazor Server (MudBlazor)
│       ├── Features/                        # Páginas Razor y Servicios HTTP por módulo
│       ├── Shared/                          # Layouts, AppBar, NavMenu
│       └── wwwroot/                         # Recursos estáticos
├── docker-compose.yml                       # Contenedor PostgreSQL 16 local
└── SIGA.sln                                 # Solución que agrupa los 7 proyectos
```

---

## 2. Convenciones de Idioma y Nomenclatura

Para garantizar coherencia y eliminar la fricción entre el negocio universitario y los estándares de la industria .NET:

1. **Dominio, Agregados y Casos de Uso:** En **Español**.
   - Entidades: `Alumno`, `Propuesta`, `PropuestaVersion`, `Inscripcion`, `Certificado`, `Autoridad`.
   - Propiedades de negocio: `NumeroDocumento`, `FechaInicio`, `MaximoAlumnos`, `TituloCertificado`.
   - Comandos / Consultas: `CrearPropuestaBorradorCommand`, `AprobarPreinscripcionCommand`.
2. **Plomería Técnica, Infraestructura y Proyectos:** En **Inglés**.
   - Interfaces transversales: `IUseCaseDispatcher`, `IUseCaseHandler`, `IUnitOfWork`, `IRepository<T>`.
   - Infraestructura y utilitarios: `ExceptionMiddleware`, `DependencyInjection`, `ApplicationUser`.
3. **Correspondencia Obligatoria Archivo $\leftrightarrow$ Tipo:**
   - Cada archivo físico debe llamarse **exactamente igual** a la clase, interfaz o enum principal que declara (prohibido que `LandingPage.cs` declare `PropuestaWeb` o `PropuestaDTO.cs` declare `CrearPropuestaResponse`).
4. **Reglas de Capitalización (C#):**
   - **PascalCase**: Clases, records, structs, interfaces, enums, métodos, propiedades y nombres de archivo.
   - **camelCase**: Parámetros de métodos y variables locales.
   - **_camelCase (con prefijo guión bajo)**: Campos privados de solo lectura inyectados en constructores (ej. `private readonly IUnitOfWork _unitOfWork;`).
   - **MAYÚSCULAS_CON_GUION_BAJO**: Constantes de Claims de seguridad (ej. `PROPUESTA.PUBLICAR`).

---

## 3. Convenciones de Mapeo (Riok.Mapperly)

- **Librería Exclusiva:** `Riok.Mapperly`. Queda desinstalado `Mapster` y prohibido el uso de `AutoMapper`.
- **Estructura de Clases de Mapeo:**
  - Se declaran como clases estáticas parciales decoradas con `[Mapper]`.
  - Ubicación: dentro de la misma carpeta del caso de uso o feature donde se consume el DTO.
  - Nombre de archivo: sufijo `Mapper` (ej. `PropuestaMapper.cs`).
  - Nombre de métodos: `[TipoOrigen]To[TipoDestino]` (ej. `PropuestaToPropuestaResponseDto(Propuesta entidad)`).

---

## 4. Estrategia de Validación y Manejo de Errores

### 4.1 Validaciones en Dos Niveles
- **Nivel Dominio (Invariantes de Agregado):** Validaciones de integridad esencial ejecutadas dentro de las entidades y factories mediante código imperativo en C#. Ante una violación, lanzan `DomainException`.
- **Nivel Aplicación (Formato y Entrada HTTP):** Validaciones de campos obligatorios, formatos de correo, longitudes y rangos mediante **FluentValidation** (`AbstractValidator<TCommand>`) antes de que el comando llegue al handler.

### 4.2 Manejo de Errores y Formato de Respuesta
- **Result Pattern (`AppResult<T>`):** Para flujos de negocio donde el resultado esperado puede ser una falla controlada previsible (ej. cupos no disponibles), retornando un resultado tipado sin arrojar excepciones.
- **Middleware Global (`ExceptionMiddleware`):** Captura todas las excepciones no controladas en WebApi y las traduce al estándar de la industria **RFC 7807 ProblemDetails** con código HTTP acorde:
  - `DomainException` / `ValidationException` $\rightarrow$ `HTTP 400 Bad Request`.
  - `NotFoundException` $\rightarrow$ `HTTP 404 Not Found`.
  - `UnauthorizedAccessException` $\rightarrow$ `HTTP 401 / 403`.
  - Excepciones no controladas $\rightarrow$ `HTTP 500 Internal Server Error` (ocultando detalles técnicos sensibles en entornos productivos).

---

## 5. Convenciones de Pruebas (Testing)

- **Framework:** `xUnit` + `FluentAssertions` + `Moq`.
- **Organización de Tests:**
  - `tests/SIGA.Domain.UnitTests/`: Pruebas de unidad puras sobre el ciclo de vida de agregados (`PropuestaVersion`, `Certificado`, `Inscripcion`, cálculo de cupos y actividad de alumnos).
  - `tests/SIGA.Application.UnitTests/`: Pruebas de casos de uso mockeando los contratos de repositorio y `IUnitOfWork`.
- **Cobertura Mínima Esperada:** $\ge 70\%$ en la lógica de negocio de Dominio y Validadores de Aplicación.

---

## 6. Convenciones de Documentación Técnica

- **Ubicación Oficial:** Toda la documentación viva de arquitectura, decisiones y operación para humanos e IAs se centraliza en **`agent/docs/`**.
- **Nomenclatura Numerada:**
  - `00-overview.md`
  - `01-glossary.md`
  - `02-user-stories.md`
  - `03-architecture.md`
  - `04-decisions.md`
  - `05-conventions.md`
  - `06-roadmap.md`
  - `07-conflicts.md`
- **Diagramas:** Todos los esquemas estructurales y de secuencia deben construirse exclusivamente con sintaxis **Mermaid** estándar compatible con GitHub y visualizadores Markdown.

---

## 7. Política de Control de Versiones (Git)

- **Gestión Soberana del Usuario:** El control de versiones, creación de ramas, fusiones y commits es gestionado **exclusivamente por el usuario de forma directa y personal**.
- **Restricción para Agentes IA:** Las herramientas de IA no deben proponer ni ejecutar comandos automatizados de Git (`git commit`, `git checkout`, `git push`, etc.), respetando la soberanía del desarrollador sobre su repositorio.

---

## 8. Política de Secretos y Configuración

- **Prohibición Absoluta:** Ningún archivo versionado en el repositorio debe contener contraseñas ni cadenas de conexión con credenciales reales en texto plano.
- **Entorno Local:** Las credenciales de desarrollo se mantienen temporalmente en `appsettings.Development.json` excluido del control público o se migran a `dotnet user-secrets`.
- **Entornos Productivos:** Las credenciales de base de datos y llaves de firma JWT se inyectan estrictamente a través de **Variables de Entorno del Sistema o del Contenedor Docker**.
