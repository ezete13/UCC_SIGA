# Backlog de Tareas (Roadmap Accionable)

> Estado actual: Inicio de Fase 4 (Generación de contexto de IA) completado. Próximos pasos a definir por el Tech Lead.

## To-Do
- [x] **Configuración Inicial**: Limpiar `Mapster` e integrar `Riok.Mapperly` en el proyecto.
- [x] **Migraciones EF Core**: Corregir y regenerar las migraciones de Identity para usar PK de tipo `int`.
- [x] **Dominio**: Asegurar que `PropuestaVersionId` sea el target real de `Inscripcion` y `Preinscripcion`.
- [x] **Dominio**: Hacer públicos los setters en `EntityBase` (solicitado en Fase 1).
- [x] **Refactor**: Remover lógica de estados y temporalidad de `Propuesta` y pasarlo 100% a `PropuestaVersion`.
- [x] **Seguridad**: Configurar JWT Auth conectando con Identity.
- [x] **Infraestructura**: Configurar templates de Scriban y PuppeteerSharp para la generación de Certificados.

## En Progreso
- [ ] Preparación del entorno y plantillas del agente (Fase 4).

## Completado
- [x] Fase 0: Inventario y lectura del repositorio.
- [x] Fase 1: Auditoría de errores, contradicciones y deuda técnica.
- [x] Fase 2: Preguntas de decisión técnica y de negocio.
- [x] Fase 3: Síntesis y documentación de arquitectura (`agent/docs/`).
- [x] Fase 4: Generación de archivos de contexto para el Agente (`agent/`).
