# 00 — Visión General del Proyecto UCC_SIGA

## 1. Identificación y Contexto

- **Nombre del Sistema:** UCC_SIGA (Sistema Integral de Gestión Académica — Módulo de Extensión y Educación Continua).
- **Institución:** Universidad Católica de Cuyo (UCC).
- **Entorno Tecnológico:** .NET 10 (C#), ASP.NET Core Web API, Blazor InteractiveServer (MudBlazor), PostgreSQL 16, Entity Framework Core 10.
- **Relación con Sistemas Existentes:** El sistema convive de forma complementaria con el sistema académico central de la universidad (SIU Guaraní). Mientras SIU Guaraní administra las carreras de grado formal, planes de estudio ministeriales y actas de examen reglamentarias, **UCC_SIGA** asume con total soberanía y agilidad la oferta no formal, diplomaturas, cursos de actualización profesional, talleres, capacitaciones y jornadas de extensión.

---

## 2. Propósito y Propuesta de Valor

El propósito de UCC_SIGA es digitalizar y descentralizar integralmente el ciclo de vida de la oferta académica de extensión universitaria:

1. **Gestión Descentralizada por Unidad Académica:**
   La universidad se estructura en facultades y sedes (Ciencias Económicas, Derecho, Ciencias Médicas, Filosofía, etc.). El sistema implementa un control de acceso de doble dimensión que garantiza que cada operador acceda exclusivamente a las propuestas y alumnos de las unidades para las que está autorizado.

2. **Modelo de Cohortes por Versiones (`PropuestaVersion`):**
   Las propuestas académicas madres (ej. *Diplomatura en Inteligencia Artificial*) permanecen publicadas y vigentes como catálogo institucional continuo, mientras que las cohortes temporales de cursado se instancian como versiones independientes con sus propias fechas, aranceles, cupos y cuerpo docente.

3. **Admisión Segura y Trazable en Dos Fases:**
   Separación rigurosa entre la postulación pública (**Preinscripción**) y la formalización académica (**Inscripción** vinculada a un registro único maestro de **Alumno**).

4. **Certificación Digital Confiable e Inalterable:**
   Generación automatizada de certificados en formato PDF (utilizando plantillas HTML en Scriban y motor PuppeteerSharp) con código QR y verificación pública mediante hash criptográfico SHA-256 de 64 caracteres. Prohibición de eliminación física y auditoría obligatoria mediante versionado histórico.

---

## 3. Actores y Usuarios del Sistema

| Actor / Rol | Descripción y Responsabilidades |
|---|---|
| **Aspirante / Comunidad** | Usuario externo que consulta la oferta académica en las páginas públicas y completa el formulario de preinscripción online. |
| **Alumno** | Estudiante registrado que posee al menos una inscripción formal (activa o histórica) en una o más propuestas académicas de la institución. |
| **Administrativo de Unidad** | Personal de extensión que carga borradores de propuestas, gestiona el contenido web, valida los datos de aspirantes, confirma el cobro administrativo y formaliza inscripciones dentro de sus unidades habilitadas. |
| **Superadministrador / Seguridad** | Responsable de la gestión de usuarios internos, asignación de roles, permisos de grano fino (claims) y vinculación de unidades académicas a operadores. |
| **Autoridad Institucional** | Decanos, secretarios académicos o directores habilitados para firmar diplomas y certificados oficiales según ciclo lectivo y facultad. |
| **Docente** | Profesional asignado al dictado académico de una cohorte o versión de propuesta específica. |

---

## 4. Estado Actual del Repositorio (Línea Base de Auditoría)

- **Fase del Proyecto:** Proyecto a medio terminar recibido en estado de auditoría inicial.
- **Backend (`SIGA.WebApi`):** Controladores REST iniciales desarrollados, actualmente en proceso de desacoplamiento de infraestructura y persistencia hacia Clean Architecture pura con CQRS (mediador nativo sin MediatR).
- **Frontend Administrativo (`SIGA.WebApp`):** Desarrollado con Blazor Server y componentes MudBlazor. Cuenta con pantallas funcionales preliminares (`CrearPropuesta.razor`, `ListarUnidades.razor`) y stubs preparados para las grillas de gestión (`InicioPropuesta`, `ListaPropuesta`, `DetallePropuesta`).
- **Portal Público (Landing Pages):** Se desarrollará en un proyecto independiente basado en el framework Astro, el cual consumirá exclusivamente los endpoints públicos de la API (`GET /api/propuestas/web`).
- **Base de Datos (`SIGA.Persistence`):** Modelada en PostgreSQL 16 con esquema `siga` y migraciones de Entity Framework Core. En proceso de consolidación tras la resolución de inconsistencias de ciclo de vida e incorporación de ASP.NET Core Identity.
