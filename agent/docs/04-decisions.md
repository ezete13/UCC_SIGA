# 04 — Registros de Decisiones de Arquitectura (ADRs)

Este documento registra formalmente las decisiones arquitectónicas tomadas para **UCC_SIGA**, documentando el contexto que originó cada decisión, las alternativas analizadas y sus consecuencias directas.

---

## ADR-001: Adopción de Clean Architecture pura con Inversión de Dependencias

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** En el código inicial recibido para auditoría, el proyecto `SIGA.Application` referenciaba directamente a `SIGA.Persistence` e inyectaba `ApplicationDbContext` en los handlers, violando la inversión de dependencias y acoplando la lógica de negocio a Entity Framework Core y a la base de datos.
- **Decisión:** Implementar Clean Architecture estricta. El núcleo `SIGA.Domain` declara los contratos de repositorio (`SIGA.Domain/Interfaces/Repositories/`) y `IUnitOfWork`. `SIGA.Application` interactúa exclusivamente con el dominio y delega lecturas complejas a `QueryServices`. `SIGA.Persistence` implementa los contratos de repositorio y de lectura. Se elimina la referencia de proyecto de Application a Persistence.
- **Alternativas Evaluadas:**
  - *Opción A (Vertical Slice pragmático sin abstracciones)*: Mantener `ApplicationDbContext` directamente en Application. Descartada para evitar que la lógica de aplicación dependa de la tecnología de persistencia.
  - *Opción B (Clean Architecture tradicional con repositorios genéricos)*: Repositorios genéricos tipo `IRepository<T>` con `IQueryable`. Descartada por fugar detalles de consulta hacia las capas superiores.
- **Consecuencias:**
  - *Positivas:* Testabilidad unitaria de Application con mocks simples, independencia absoluta del motor de BD y cumplimiento de principios SOLID.
  - *Negativas:* Mayor cantidad de interfaces y clases de mapeo entre capas.

---

## ADR-002: Desacoplamiento de Oferta Académica y Cohortes mediante `PropuestaVersion`

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** El modelo original obligaba a archivar la propuesta para poder emitir certificados. Si una propuesta se archivaba, no podía recibir más alumnos, lo que impedía que una carrera o curso con múltiples ediciones anuales permaneciera visible y activo en el catálogo institucional.
- **Decisión:** Dividir el concepto en dos entidades:
  1. `Propuesta` (Catálogo general / madre, con título, unidad académica y contenido web). Permanece activa en el tiempo.
  2. `PropuestaVersion` (Edición / cohorte temporal, con fechas de cursado, cupos máximos, aranceles, ciclo lectivo y cuerpo docente). Los estados de ciclo de vida (`Borrador`, `Publicada`, `Archivada`) residen en la versión. Las inscripciones y certificaciones se asocian a la versión.
- **Alternativas Evaluadas:**
  - *Opción A (Propuesta única con clonación de registros)*: Duplicar la propuesta completa cada año. Descartada por generar inconsistencia en URLs web, duplicación de temarios y pérdida de trazabilidad histórica.
  - *Opción B (Versión embebida en Propuesta)*: Arreglo de fechas dentro de la misma propuesta. Descartada por complicar las relaciones relacionales de inscripción.
- **Consecuencias:**
  - *Positivas:* La propuesta madre nunca pierde vigencia en el portal público; permite certificar cohortes concluidas sin cerrar la oferta futura; cupos administrados limpiamente por versión.
  - *Negativas:* Requiere una tabla adicional (`propuestas_versiones`) y actualizar las FKs de preinscripciones e inscripciones.

---

## ADR-003: Mediador de Casos de Uso Propio (Sin MediatR)

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** Se requiere desacoplar los controladores WebApi de los manejadores de casos de uso sin incorporar dependencias pesadas de terceros que introduzcan complejidad innecesaria en el pipeline.
- **Decisión:** Implementar un despachador nativo propio (`UseCaseDispatcher`) que implementa `IUseCaseDispatcher` resolviendo los handlers genéricos `IUseCaseHandler<TRequest, TResponse>` directamente desde el `IServiceProvider` de ASP.NET Core.
- **Alternativas Evaluadas:**
  - *Opción A (Librería MediatR)*: Estándar extendido pero agrega dependencias externas, sobrecarga de paquetes y licenciamiento variable en versiones recientes.
  - *Opción B (Inyección directa de cada CommandHandler en controladores)*: Sin mediador. Descartada por sobrecargar los constructores de los controladores con múltiples dependencias.
- **Consecuencias:**
  - *Positivas:* Código 100% bajo control del equipo, cero dependencias NuGet externas para mensajería interna, máximo rendimiento y control directo sobre el ciclo de vida de los servicios (Scoped).
  - *Negativas:* Funcionalidades avanzadas (como behaviors de pipeline) deben escribirse manualmente si se requieren.

---

## ADR-004: Herramienta de Mapeo Oficial: Riok.Mapperly (Desinstalación de Mapster)

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** En el archivo de arquitectura se exigía el uso exclusivo de `Riok.Mapperly`, pero en `SIGA.Application.csproj` se había instalado `Mapster`.
- **Decisión:** Desinstalar `Mapster` e implementar **Riok.Mapperly** como biblioteca oficial y obligatoria de mapeo objeto-objeto en compile-time.
- **Alternativas Evaluadas:**
  - *Opción A (Conservar Mapster)*: Muy rápido, pero realiza mapeos en runtime basados en expresiones dinámicas.
  - *Opción B (AutoMapper)*: Prohibido expresamente por degradación de performance y dificultades de trazabilidad.
  - *Opción C (Mapeo manual con métodos de extensión)*: Verbosidad excesiva y mantenimiento manual costoso.
- **Consecuencias:**
  - *Positivas:* Código de mapeo generado en tiempo de compilación (*Source Generators*), cero overhead en tiempo de ejecución, advertencias en compilación ante campos no mapeados y consistencia con los estándares corporativos.
  - *Negativas:* Exige que las clases de mapeo se declaren como `partial`.

---

## ADR-005: Generación de Certificados en PDF con Scriban y PuppeteerSharp

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** Existía un archivo `certificado.html` con maquetación CSS precisa y sintaxis de tokens (`{{ alumno }}`, `{% for f in firmas %}`), pero sin motor de generación implementado.
- **Decisión:** Integrar en `SIGA.Infrastructure`:
  1. **Scriban:** Como motor de plantillas para renderizar el HTML procesando las variables y bucles de firmas de forma nativa y asíncrona.
  2. **PuppeteerSharp:** Como motor de impresión headless (Chromium) para convertir el HTML resultante a PDF preservando tipografías locales (Montserrat), imágenes institucionales vectoriales y código QR embebido.
- **Alternativas Evaluadas:**
  - *Opción A (QuestPDF)*: Excelente rendimiento, pero exigía reprogramar en código C# toda la maquetación visual existente del certificado HTML/CSS.
  - *Opción B (iTextSharp / DinkToPdf)*: Librerías obsoletas o con licencias comerciales restrictivas.
- **Consecuencias:**
  - *Positivas:* Aprovechamiento directo de la plantilla HTML/CSS ya diseñada por el equipo gráfico; calidad de impresión vectorial exacta y soporte completo de tipografías web y QR.
  - *Negativas:* PuppeteerSharp requiere la disponibilidad de un binario de Chromium (descarga en primer arranque o imagen Docker preconfigurada).

---

## ADR-006: Integridad Criptográfica SHA-256 para Verificación de Certificados

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** El código original seteaba el atributo `HashSeguridad` con la cadena fija `"HashSeguridad"`, imposibilitando la validación de autenticidad de los certificados emitidos.
- **Decisión:** Implementar la generación de un hash criptográfico **SHA-256** de 64 caracteres en hexadecimal calculado a partir de los atributos inmutables del certificado (`Id`, `Uuid`, `AlumnoId`, `PropuestaVersionId`, `FechaEmision` y `Token`). Este hash se almacena en la base de datos y se embebe en el código QR del PDF.
- **Alternativas Evaluadas:**
  - *Opción A (Guid simple en QR)*: Permite consultar el registro, pero no garantiza que los datos impresos no hayan sido adulterados externamente.
  - *Opción B (Firma digital con certificado X.509 PKI)*: Máxima seguridad legal, pero compleja de implementar en el MVP por el costo y gestión de certificados de servidor.
- **Consecuencias:**
  - *Positivas:* Detección inmediata de fraudes o alteraciones de documentos impresos/digitales mediante recálculo del hash en la página pública de verificación.
  - *Negativas:* No sustituye a una firma digital calificada de ley nacional (se evaluará en fases posteriores).

---

## ADR-007: Módulo de Seguridad con ASP.NET Core Identity sobre PostgreSQL con Clave `int`

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** Se requería implementar el módulo de seguridad con claims y restricción por unidad académica sin acceso actual al servidor de Single Sign-On corporativo (Keycloak).
- **Decisión:** Implementar **ASP.NET Core Identity** sobre PostgreSQL con clave entera (`IdentityUser<int>`, `IdentityRole<int>`), enriquecido con una tabla relacional `usuarios_unidades` (`UsuarioId` $\leftrightarrow$ `UnidadId`) y un esquema de Claims para los permisos operativos. La capa de presentación utilizará tokens JWT y se creará la abstracción `IProveedorIdentidad` para posibilitar una transición transparente a Keycloak u OAuth2 en el futuro.
- **Alternativas Evaluadas:**
  - *Opción A (Identity con clave Guid/String predeterminada)*: Descartada para mantener consistencia estricta con todas las entidades del modelo que usan `int Id`.
  - *Opción B (Autenticación personalizada desde cero)*: Descartada por no reinventar la rueda en hashing de contraseñas, tokens de reseteo y lockout de seguridad.
- **Consecuencias:**
  - *Positivas:* Seguridad robusta estándar de Microsoft, soporte nativo de Claims, compatibilidad total con EF Core y camino despejado para migrar a SSO empresarial más adelante.
  - *Negativas:* `ApplicationDbContext` debe derivar de `IdentityDbContext`.

---

## ADR-008: Modelo de Admisión en Dos Fases con Transacción Atómica

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** Se analizó si unificar Preinscripción e Inscripción en una sola entidad con 5 estados o conservar la separación relacional en dos entidades.
- **Decisión:** Conservar las dos entidades relacionales:
  1. `Preinscripcion`: Registro provisional de aspirante (no requiere que el alumno exista previamente en la base de datos de la universidad).
  2. `Inscripcion`: Matrícula académica formal ligada a un registro único maestro de `Alumno`.
  La aprobación de una preinscripción ejecuta la búsqueda/alta de alumno y la creación de inscripción dentro de una **transacción única de base de datos** (`IUnitOfWork.ExecuteTransactionAsync`).
- **Alternativas Evaluadas:**
  - *Opción A (Entidad única Inscripción)*: Descartada porque obligaría a crear alumnos en estado provisional en la base de datos por cada formulario web recibido, ensuciando la tabla maestra de personas con postulaciones rechazadas o spam.
- **Consecuencias:**
  - *Positivas:* Máxima pureza en el padrón de alumnos de la universidad y trazabilidad exacta de solicitudes rechazadas o revocadas.
  - *Negativas:* El administrativo debe ejecutar la acción explícita de aprobación para formalizar el pase.

---

## ADR-009: Desacoplamiento de Portales Públicos con Astro Framework

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** Las páginas públicas de las propuestas deben ser ultra rápidas, optimizadas para SEO y ligeras para aspirantes en dispositivos móviles, sin sobrecargar el servidor Blazor.
- **Decisión:** Desarrollar el portal público en un proyecto independiente basado en el framework **Astro**. Este proyecto consumirá exclusivamente la API REST (`SIGA.WebApi`) para consultar las propuestas publicadas y enviar los formularios de preinscripción.
- **Alternativas Evaluadas:**
  - *Opción A (Hacer la landing page dentro de Blazor WebApp)*: Descartada porque Blazor InteractiveServer requiere una conexión permanente WebSocket/SignalR por cada visitante, lo cual no escala eficientemente para tráfico masivo de marketing público ni es óptimo para rastreadores de motores de búsqueda (SEO).
- **Consecuencias:**
  - *Positivas:* Velocidad de carga insuperable (sitios estáticos / SSR ligero), SEO perfecto y desacoplamiento total de la infraestructura de administración interna.
  - *Negativas:* Requiere gestionar dos frontends independientes (Blazor para internos, Astro para la comunidad).

---

## ADR-010: Exclusión de Pasarela de Pagos Online en el MVP

- **Estado:** Aceptado
- **Fecha:** Octubre 2026
- **Contexto:** Definir cómo se gestionan los aranceles y el cobro de inscripciones en la primera versión productiva.
- **Decisión:** El sistema UCC_SIGA registrará únicamente la información tarifaria (`ImporteBase`, `Cuotas`, `ConceptoPago`) a título informativo y la aprobación de inscripciones se realizará mediante confirmación administrativa manual tras verificar el pago bancario externamente. La integración automatizada con pasarelas de pago (Mercado Pago / Plus Pagos) se difiere a la fase posterior del Roadmap.
- **Alternativas Evaluadas:**
  - *Opción A (Integrar pasarela bancaria en MVP)*: Descartada para no bloquear la salida a producción por homologaciones bancarias externas.
- **Consecuencias:**
  - *Positivas:* Independencia inmediata para entrar en producción y simplificación del modelo transaccional.
  - *Negativas:* Requiere carga administrativa manual de comprobantes en tesorería/extensión.
