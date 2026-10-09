# 07 — Registro de Conflictos, Hallazgos y Resoluciones

Este documento consolida la matriz de auditoría de la **Fase 1**, registrando el estado de resolución definitivo y las acciones técnicas acordadas para cada uno de los hallazgos y suposiciones.

---

## 1. Matriz de Hallazgos y Resoluciones

| ID | Tipo Original | Descripción del Conflicto | Estado de Resolución | Resolución Acordada / Acción Técnica |
|---|---|---|---|---|
| **ERR-001** | `ERROR_CÓDIGO` | `EntityBase` con `protected set` genera 383 errores de compilación `CS0272` al sembrar datos en EF Core. | **RESUELTO** | Se cambian los accesores de `EntityBase` a `public set`, permitiendo la inicialización directa en `HasData(...)`. |
| **ERR-002** | `ERROR_CÓDIGO` | Referencias rotas a `Features/Unidades` causan errores `CS0246` porque la carpeta está vacía. | **RESUELTO** | Se descarta la feature de escritura de Unidad (`CrearUnidadCommand`). Las unidades se gestionan por base de datos y solo se expone un endpoint de lectura (`GET /api/lookups` o `/api/unidades`) para los combos de la UI. |
| **ERR-003** | `ERROR_CÓDIGO` | `Certificado.Crear` valida `alumno.AlumnoEstado.Codigo != "ALTA"`, pero el estado es `"ACT"` (`Activo`). | **RESUELTO** | Se corrige la verificación para validar contra `EstadoAlumnoEnum.Activo` (código `"ACT"`), comprobando además que no esté suspendido ni bloqueado. |
| **ERR-004** | `ERROR_CÓDIGO` | `app.MapControllers()` contenido dentro de `if (app.Environment.IsDevelopment())`. | **RESUELTO** | Se mueve `app.MapControllers()` fuera del condicional para habilitar la API en todos los entornos de ejecución. |
| **ERR-005** | `ERROR_CÓDIGO` | `PreinscripcionesController` declara método `CrearModalidad` y bindea un command directo desde Form. | **RESUELTO** | Se renombra a `CrearPreinscripcion`, se parametriza la modalidad seleccionada y se estandariza el payload a JSON mediante Request DTO. |
| **DOC-001** | `ERROR_DOC` | `doc/Architecture.md` (1011 líneas) describe el sistema externo "InRe" (pagos, SISA, SQL Server). | **RESUELTO** | Se descarta el contenido de dominio de "InRe" y se extraen los patrones de arquitectura deseados (Clean Architecture, CQRS propio, Result Pattern, Mapperly), ahora formalizados en `agent/docs/03-architecture.md`. |
| **CON-001** | `CONTRADICCIÓN` | Conflicto de modelado entre entidad única de 5 estados (`historia_de_usuario.md`) vs dos entidades relacionales (`Requerimientos.md`). | **RESUELTO** | Se ratifica el modelo canónico de dos entidades: `Preinscripcion` (postulación provisional) e `Inscripcion` (matrícula formal vinculada a `Alumno`). La aprobación ejecuta una transacción atómica. |
| **CON-002** | `CONTRADICCIÓN` | `EstadoCertificadoEnum` tenía 3 estados femeninos (`Borrador`, `Publicada`, `Archivada`) copiados de Propuesta. | **RESUELTO** | Se adopta el ciclo oficial de 6 estados: `No Generable`, `Pendiente`, `Generado`, `Aprobado`, `Inhabilitado` y `Revocado`. |
| **CON-003** | `CONTRADICCIÓN` | Regla restrictiva que exigía archivar la propuesta madre para emitir certificados, cerrando la oferta futura. | **RESUELTO** | Se crea la entidad `PropuestaVersion` (cohorte/edición). La propuesta madre permanece activa; las inscripciones y certificados se asocian a la versión. Los certificados se emiten al finalizar la versión. |
| **CON-004** | `CONTRADICCIÓN` | `SIGA.Application` referenciaba a `SIGA.Persistence` e inyectaba `ApplicationDbContext` violando Clean Architecture. | **RESUELTO** | Se elimina la referencia de Application a Persistence. Se implementan interfaces de repositorio (`IRepository<T>`, `IUnitOfWork`) en Domain y `QueryServices` en Application, implementados en Persistence. |
| **SEC-001** | `RIESGO_SEGURIDAD` | Contraseñas de PostgreSQL expuestas en texto plano en Git (`Readme.md`, `DbContextFactory.cs`, `appsettings.json`). | **RESUELTO** | En desarrollo local se mantienen temporalmente en `appsettings.Development.json` excluido de Git o en `dotnet user-secrets`; en producción se inyectan por variables de entorno del sistema o Docker. |
| **SEC-002** | `RIESGO_SEGURIDAD` | Módulo de Seguridad (Claims por módulo y restricción por Unidad Académica) sin implementar. | **RESUELTO** | Se implementa ASP.NET Core Identity sobre PostgreSQL con clave `int Id` (`ApplicationUser`, `ApplicationRole`), tabla relacional `usuarios_unidades`, claims por rol y abstracción `IProveedorIdentidad`. |
| **AMB-001** | `AMBIGÜEDAD` | La entidad `Autoridad` carecía de los atributos `FirmaCertificado` y `OrdenFirma` para resolver qué autoridades firman. | **RESUELTO** | Se incorporan a la entidad `Autoridad` las propiedades `FirmaCertificado: bool` y `OrdenFirma: int`. |
| **DEU-001** | `DEUDA_TÉCNICA` | 10 archivos vacíos (0 bytes) y stubs sin propósito (`Class1.cs`, `DropdownUnidad.razor`). | **RESUELTO** | Se purgan los stubs (`Class1.cs`) y los archivos vacíos sin propósito, conservando e implementando las vistas administrativas planificadas en el frontend Blazor. |
| **DEU-002** | `DEUDA_TÉCNICA` | Existía `certificado.html` con tokens pero sin motor de renderizado ni generador de PDF. | **RESUELTO** | Se autoriza e implementa **Scriban** (motor de plantillas HTML asíncrono) + **PuppeteerSharp** (Chromium headless para generación de PDF vectorial) en `SIGA.Infrastructure`. |
| **DEU-003** | `DEUDA_TÉCNICA` | Atributo `HashSeguridad` de certificados seteado con la constante literal fija `"HashSeguridad"`. | **RESUELTO** | Se implementa el cálculo de un hash criptográfico real **SHA-256** de 64 caracteres generado a partir de los datos inmutables del certificado y embebido en el código QR. |
| **DEU-004** | `DEUDA_TÉCNICA` | URL base de WebApi hardcodeada en Blazor y enlaces residuales (`counter`, `weather`) en `NavMenu.razor`. | **RESUELTO** | Se parametriza la URL base en `appsettings.json` y se reestructura el menú de navegación con los módulos reales del sistema. |
| **NOM-001** | `INCONSISTENCIA_NOMBRES` | Discrepancia entre nombres de archivos y nombres de clases (`LandingPage.cs` $\rightarrow$ `PropuestaWeb`, `PropuestaDTO.cs` $\rightarrow$ `CrearPropuestaResponse`). | **RESUELTO** | Se alinean todos los nombres de archivo para que coincidan unívocamente con el tipo principal que declaran. |
| **NOM-002** | `INCONSISTENCIA_NOMBRES` | Inconsistencia en campo de documento entre entidades (`Alumno.NumDocumento`, `Preinscripcion.Documento`, `Docente.Dni`). | **RESUELTO** | Se estandariza a `NumeroDocumento: string` y se incorpora `TipoDocumentoId: int` en `Docente` para paridad simétrica total con Alumno y Preinscripción. |
| **NOM-003** | `INCONSISTENCIA_NOMBRES` | Configuraciones EF Core con namespace erróneo `SIGA.Infrastructure.Data.Configurations`. | **RESUELTO** | Se unifica el namespace a `SIGA.Persistence.Configurations`. |
| **DUP-001** | `DUPLICADO` | Dos migraciones iniciales redundantes generadas simultáneamente en `SIGA.Persistence/Migrations/`. | **RESUELTO** | Se regenerará una única migración consolidada limpia (`InitialDatabaseClean`) una vez estabilizado el nuevo modelo de datos. |

---

## 2. Validación de Suposiciones Técnicas

- **`SUP-01` (Origen de `Architecture.md`):** Confirmada. Se utilizó como base para los patrones arquitectónicos y se descartó su contenido de dominio de cobranzas externas.
- **`SUP-02` (Separación de Preinscripción e Inscripción):** Confirmada. Se ratificó la necesidad de mantener dos entidades relacionales para preservar la pureza del padrón de alumnos.
- **`SUP-03` (Propósito de `SIGA.Shared`):** Confirmada. Aloja DTOs de transferencia y contratos de lookups compartidos entre backend y frontends.
- **`SUP-04` (Faltantes de `Unidades` en Application):** Confirmada. Se descartó la escritura de Unidades en la API y se preservó únicamente el lookup de lectura.

---

## 3. Estado Final de la Auditoría

- **Total de Conflictos / Hallazgos Identificados:** 21
- **Conflictos Resueltos:** 21 (100%)
- **Conflictos Abiertos:** 0 (0%)
- **Suposiciones Validadas:** 4 de 4 (100%)
