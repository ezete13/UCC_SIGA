# SIGA - Sistema Integral de Gestión Académica

**Contexto Global**
Sistema interno de la universidad para gestionar cursos extracurriculares y jornadas (excluye carreras oficiales gestionadas por SIU Guaraní). Permite a los alumnos preinscribirse e inscribirse a versiones específicas de propuestas académicas.

**Arquitectura (Clean Architecture Pura)**
- **SIGA.Domain**: Entidades, enumeraciones, interfaces de repositorios. Independiente.
- **SIGA.Application**: Vertical Slices. Usa `MediatR` para commands/queries. Contiene la lógica de negocio.
- **SIGA.Persistence**: EF Core. Mapeo de entidades (EntityConfigurations), DbContext, migraciones.
- **SIGA.WebApi**: Controladores que exponen endpoints HTTP y delegan a Application.

**Modelo de Dominio Principal**
- `Propuesta`: Catálogo general/marketing (no tiene fechas).
- `PropuestaVersion`: Instancia temporal/cohorte de una Propuesta. Aquí viven los estados ricos (Borrador, Publicada, etc) y la relación con docentes.
- `Preinscripcion` e `Inscripcion`: Apuntan a `PropuestaVersion`.
- `ApplicationUser`: Extiende `IdentityUser<int>` (usando INT como PK obligatoriamente).

**Stack Técnico Restringido**
- .NET 10 (LTS).
- Base de datos: PostgreSQL.
- Mapeo: `Riok.Mapperly` (Mapster está vetado).
- Documentos: `Scriban` y `PuppeteerSharp` para generación de certificados PDF.
- Seguridad: ASP.NET Core Identity.
- Pagos: NO IMPLEMENTAR (delegado a sistema externo).

**Reglas Críticas de Desarrollo**
- Nunca inventar entidades/endpoints que no estén en el código/documentación.
- Todo lo relacionado con control de versiones (Git) es gestionado EXCLUSIVAMENTE por el usuario.
- Seguir la estructura de nombramiento en inglés para código.
