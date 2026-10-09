# Changelog

Todos los cambios notables en este proyecto serán documentados en este archivo.
El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/).

## [Unreleased]

### Changed
- [T-07] **Infraestructura**: Se integró el paquete de NuGet `PuppeteerSharp` y `Scriban`, y se creó la interfaz e implementación de `IGeneradorPdfService` para renderizar PDFs de certificados desde plantillas HTML de manera nativa y asíncrona.
- [T-06] **Seguridad**: Se configuró la autenticación mediante JWT Bearer tokens en WebApi, se creó la abstracción `IProveedorIdentidad` para encapsular la lectura de Claims y asegurar el desacoplamiento, y se vinculó la autenticación al middleware.
- [T-02] **Infraestructura**: Se implementó ASP.NET Core Identity sobre PostgreSQL utilizando llaves primarias numéricas (`int`), configurando `ApplicationDbContext` y generando la migración consolidada `InitialDatabaseClean`.
- [T-01] **Infraestructura**: Se reemplazaron las dependencias de `Mapster` y `Mapster.DependencyInjection` por `Riok.Mapperly` en la capa de Aplicación según las directivas de arquitectura ADR-003.
- [T-03] **Domain**: Se ajustaron las entidades `Inscripcion` y `Preinscripcion` para referenciar a `PropuestaVersionId` en lugar de `PropuestaId`, completando el enrutamiento de alumnos hacia las cohortes y no hacia el catálogo base.
- [T-05] **Refactor**: Separación arquitectónica de `Propuesta` (Catálogo) y `PropuestaVersion` (Cohorte). Se removieron las lógicas temporales y de estado de `Propuesta` purificando el Dominio.
- [T-04] **Domain**: Se cambiaron los modificadores `protected set` y `private set` por `public set` en `EntityBase`, `Unidad` y `Propuesta` para resolver los errores de compilación `CS0272` y `CS0200` durante el sembrado de datos en las configuraciones de EF Core.
- **Bugfix**: Se eliminó `UnidadController.cs` y su registración en `DependencyInjection.cs` ya que la feature había sido descartada, causando errores de compilación por referencias huérfanas (ERR-002).
