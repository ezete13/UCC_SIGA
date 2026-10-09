# Cómo debe trabajar una IA en este repositorio (SIGA)

## 📌 Reglas Fundamentales
1. **No inventar**: Si una entidad, archivo, endpoint o tabla no existe explícitamente en el repositorio, no asumas su existencia. Pregunta al usuario.
2. **Consultar Documentación**: Antes de implementar, siempre lee `agent/CONTEXT.md` y revisa los archivos de `agent/docs/` si necesitas más detalles (especialmente `03-architecture.md` y `05-conventions.md`).
3. **No gestionar Control de Versiones**: El usuario gestiona Git manualmente. NO crees ramas, NO hagas commits y NO generes archivos relacionados a Git.
4. **Respetar Clean Architecture**: 
   - `SIGA.Domain`: No depende de nadie (Soberano). Contiene Entidades e Interfaces.
   - `SIGA.Application`: Vertical Slices. CQRS básico, casos de uso.
   - `SIGA.Persistence`: EF Core, implementaciones de repositorios.
   - `SIGA.WebApi`: Controladores y configuración.
5. **Tecnologías Estrictas**:
   - .NET 10 (LTS).
   - PostgreSQL + EF Core.
   - `Riok.Mapperly` (Prohibido usar Mapster).
   - Identity (`ApplicationUser : IdentityUser<int>`).

## 📖 Orden de Lectura Sugerido
1. `agent/CONTEXT.md` (Punto de entrada obligatorio).
2. `agent/docs/03-architecture.md` (Para entender las capas).
3. `agent/docs/05-conventions.md` (Para naming y convenciones).
4. `agent/TASKS.md` (Para saber en qué estamos trabajando).

## 🚫 Prohibiciones
- **NO** modificar la entidad `PropuestaVersion` asumiendo que es `Propuesta`. Las propuestas son el catálogo (Marketing), las versiones son las cohortes.
- **NO** implementar manejo de pagos (existirá una API externa futura para esto).
- **NO** usar Tailwind en web apps (salvo que se pida y se aclare versión).
- **NO** implementar UI/Landing pages aquí (se harán en Astro en el futuro).

## ✅ Checklist antes de tocar código
- [ ] ¿He revisado si la entidad ya existe en `SIGA.Domain`?
- [ ] ¿He confirmado que la modificación respeta la Clean Architecture?
- [ ] ¿Estoy usando `.int` para las Primary Keys de Identity?
- [ ] ¿La respuesta sugerida es verificable en el código o requiere asunciones? (Si es lo segundo, preguntar).
