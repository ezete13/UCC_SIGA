# Arquitectura del Sistema SIGA

## 1.1 Visión General

La solución **SIGA** sigue los principios de **Clean Architecture** combinada con **CQRS** y **Vertical Slice Architecture**. El sistema está diseñado para ser independiente de proveedores externos (SISA, adaptadores) y mantener un alto grado de mantenibilidad y testabilidad.

## 1.2 Estructura de Proyectos .NET

La solución está compuesta por los siguientes proyectos, manteniendo dependencias unidireccionales:

| Proyecto | Capa | Dependencias | Descripción |
|----------|------|--------------|-------------|
| **`SIGA.Domain`** | Núcleo | Ninguna | Contiene entidades, enums, valueObjects, interfaces de repositorio. No tiene dependencias externas. |
| **`SIGA.Application`** | Aplicación | `SIGA.Domain` | Implementa casos de uso (Commands/Queries) usando CQRS con handlers propios. Contiene DTOs, validaciones FluentValidation y lógica de aplicación. |
| **`SIGA.Persistence`** | Persistencia | `SIGA.Application` | Contiene `DbContext`, configuraciones EF Core `IEntityTypeConfiguration<T>`, repositorios concretos y migraciones. Capa específica de acceso a datos. |
| **`SIGA.Infrastructure`** | Infraestructura | `SIGA.Application` | Implementación de clientes HTTP externos, adaptadores, schedulers `IHostedService`, interceptors y configuración de servicios de terceros (SISA, Gravitee). |
| **`SIGA.Api`** | Presentación | `SIGA.Application` | Punto de entrada HTTP. Controladores REST, composición root DI, configuración Swagger/OpenAPI, middleware de excepción y autenticación. |
| **`SIGA.Jobs`** | Worker Service | `SIGA.Application` + `SIGA.Infrastructure` | Proyecto de segundo plano que ejecuta tareas programadas. Contiene el `HandsFree` para reprocesar cobranzas y otros procesos en lote. |

## 1.3 Patrones de Diseño Aplicados

### 1.3.1 Clean Architecture
- Separación de preocupaciones en capas concéntricas
- Dependencias solo hacia el interior (inversión de dependencias)
- La capa Domain no conoce ninguna capa externa

### 1.3.2 CQRS (Command Query Responsibility Segregation)
- **Commands:** Representan intenciones de modificar estado. Cada Command tiene un Handler asociado.
- **Queries:** Representan operaciones de lectura. Cada Query tiene un Handler asociado.
- Se mantiene una sola base de datos de escritura y lectura, pero el CQRS se mantiene para separation de preocupaciones.
- **No se usa librería MediatR:** Se implementa un Mediator propio sobre el DI nativo de ASP.NET Core.

### 1.3.3 Vertical Slice
- El código se organiza alrededor de funcionalidades/casos de uso en lugar de capas técnicas
- Cada feature (caso de uso) tiene su propio folder con handler, validator, DTOs y mapeador
- Mejora la descubribilidad y reduce el acoplamiento entre features

### 1.3.4 Repository Pattern
- Interfaces definen los métodos de interacción con la BD en la capa Domain
- Implementaciones concretas viven en la capa Persistence
- Desacoplan la lógica de negocio de los detalles de la base de datos

### 1.3.5 Riok.Mapperly (Mapeo Objeto-Objeto)
- **Exclusivo:** Se usa exclusivamente `Riok.Mapperly` para mapeo DTO → Entidad y viceversa.
- **Prohibido:** AutoMapper y mapeo manual masivo quedan estrictamente prohibidos.
- Los mapeadores se crean como clases parciales decoradas con `[Mapper]` atributo.
- Ubicación: dentro del mismo proyecto donde se define el DTO destino (por conveniencia, respetando límites de Clean Architecture).
- Convención de nomenclatura: sufijo `Mapper` en el nombre del archivo (ej: `CobranzaMapper.cs`).
- Estructura de métodos: `[ObjetoOrigen]To[ObjetoDestino]` (ej: `CobranzaToCobranzaDto`).

### 1.3.6 State Machine (Manejo de Estados)
- Las cobranzas y rendiciones siguen patrones de máquinas de estados definidas.
- Las transiciones de estado están validadas en la capa Domain (código imperativo, no FluentValidation).
- Los estados válidos y transiciones están definidos en los documentos de dominio.

### 1.3.7 Unit of Work
- La capa Persistence implementa `IUnitOfWork` para transacciones consistentes.
- Agrupa operaciones de escritura en una sola transacción de BD.

## 1.4 Tecnologías y Configuración

| Componente | Tecnología |
|------------|------------|
| **Plataforma** | .NET 8 (o superior) |
| **ORM** | Entity Framework Core 8 (Code-First, Migraciones) |
| **Base de Datos** | Postgres 2022 |
| **API Gateway** | Gravitee.io (desplegado por la empresa) |
| **Autenticación/Autorización** | JWT (Bearer Tokens). Gravitee gestiona la autenticación contra IdP corporativo (Azure AD/Keycloak) y pasa un JWT a la API de SIGA. |
| **Manejo de Estados** | Pattern de State Machine para cobranzas y rendiciones |
| **Documentación** | Swagger/OpenAPI 3.0 para la API pública |
| **Logging** | Serilog |
| **Scheduler** | `IHostedService` propio (no Hangfire/Quartz) |
| **Validaciones** | FluentValidation (capa Application) + Reglas de invariantes (capa Domain) |

## 1.5 Convenciones de Código

### 1.5.1 Nomenclatura
- Variables, constantes, clases, propiedades, métodos: **español**
- Nombres de proyectos: **Inglés** (SIGA.Domain, SIGA.Application, etc.)
- Nombres de archivos y clases: Sufijo `Mapper` para clases de mapeo
- Métodos de mapeo: Estructura `[ObjetoOrigen]To[ObjetoDestino]`
- Enums: Sufijo `E` prefijo descriptivo (ej: `EEstadoCobranza`, `EMedioPago`)

### 1.5.2 Manejo de Errores
- Las entidades/domain producen errores controlados (usando ErrorOr pattern o resultados tipados)
- La capa API transforma errores a HTTP ProblemDetails
- Excepciones no controladas se manejan mediante middleware global
- **No se lanzan excepciones para errores de negocio esperables**

### 1.5.3 Validaciones
- **Dominio (Domain):** Reglas invariantes que protegen la integridad del agregado (código imperativo en métodos del dominio)
- **Aplicación (Application):** Validaciones de entrada, formato, campos obligatorios (FluentValidation)
- **Presentación (API):** Validaciones propias de HTTP/protocolo

### 1.5.4 Inyección de Dependencias
- Cada proyecto determina su propia clase de inyección de dependencias
- El composition root está en la capa API (`Program.cs` o `Startup.cs`)
- Secuencia: `services.AddApplication(); services.AddInfrastructure(configuration);`

### 1.5.5 Respuesta de API
- Endpoints RESTful con convenios consistentes
- Uso de Swagger/OpenAPI 3.0 para documentación automática
- Health checks integrados
- CORS configurado para consumidores autorizados

## 1.6 Decisiones de Arquitectura Definitivas

### 1.6.1 Mediator Propio (Sin MediatR)
- **Decisión:** Implementar un mediador propio sobre el DI nativo de ASP.NET Core, sin depender de la librería MediatR.
- **Justificación:** Mantiene la arquitectura simple y bajo control del equipo. Evita dependencias externas innecesarias y permite un control fino del ciclo de vida de los handlers.

### 1.6.2 Persistencia Separada (Capa SIGA.Persistence)
- **Decisión:** Considerar `SIGA.Persistence` como capa definitiva separada de `SIGA.Infrastructure`.
- **Justificación:** Persistence contiene específicamente la persistencia de datos (DbContext, configuraciones EF, repositorios). Infrastructure contiene implementación de servicios externos (clientes HTTP, adaptadores). Esta separación clara evita superposición de responsabilidades.

### 1.6.3 Herramienta de Mapeo: Riok.Mapperly
- **Decisión:** Riok.Mapperly como herramienta oficial de mapeo. No se usa AutoMapper ni mapeo manual excepto casos puntuales donde Mapperly no sea conveniente.
- **Justificación:** Performance mejorada sobre AutoMapper, código generado en compile-time, tipo fuerte y menos error pronóstico.

### 1.6.4 Scheduler: IHostedService Propio
- **Decisión:** Implementación propia de `IHostedService`/`BackgroundService` para jobs programados.
- **Justificación:** Mantiene menos dependencias y mayor control sobre el procesamiento asíncrono. No se agrega Hangfire/Quartz si las necesidades actuales se pueden cubrir con la propia aplicación.

### 1.6.5 Base de Datos: Postgres
- **Decisión:** SQL Server 2022 como proveedor definitivo.
- **Justificación:** Decisión empresarial definida. El sistema no es agnóstico de proveedor en esta etapa.





## Seguridad

### Autenticación
La API de Pagos usa **OAuth 2.0 Client Credentials** con **JWT firmados (RS256)**
emitidos por Keycloak. La validación es local (JWKS), sin round-trip al IdP.

- IdP: Keycloak (`https://auth.example.com/realms/payments`)
- Algoritmo: RS256
- TTL del token: 5 min
- Header: `Authorization: Bearer <jwt>`

### Autorización
Roles definidos en Keycloak y validados con `[Authorize(Roles = "...")]`:


### Configuración (.NET)
[bloque de código de AddJwtBearer]

### Secretos
`client_secret` en Key Vault, rotación cada 90 días. Nunca en Git.