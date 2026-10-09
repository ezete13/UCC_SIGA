# 02 — Historias de Usuario con Criterios de Aceptación

Todas las historias siguen el formato estándar Connextra, con Criterios de Aceptación especificados en sintaxis Gherkin (Dado/Cuando/Entonces) y Definition of Done (DoD). Las funcionalidades previstas para fases posteriores se señalan explícitamente como `[PENDIENTE]`.

---

## ÉPICA 1: MÓDULO DE SEGURIDAD Y CONTROL DE ACCESO

### US-SEG-01: Autenticación de Usuarios Internos
- **Como** usuario administrativo de la universidad,
- **Quiero** iniciar sesión con mis credenciales (email y contraseña),
- **Para** acceder a los paneles de gestión según mis permisos autorizados.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Inicio de sesión exitoso
  Dado que existe un usuario registrado activo con email "operador@ucc.edu.ar" y clave válida
  Cuando el usuario envía su email y contraseña al endpoint de login
  Entonces el sistema responde con código HTTP 200 y emite un Access Token JWT válido por 60 minutos junto a un Refresh Token
  Y el payload del token contiene los claims de roles, permisos y las unidades académicas asignadas.

Escenario: Credenciales inválidas
  Dado un usuario registrado en el sistema
  Cuando intenta iniciar sesión con una contraseña incorrecta
  Entonces el sistema responde con código HTTP 401 Unauthorized sin detallar cuál de los dos datos falló.

Escenario: Usuario desactivado
  Dado un usuario cuyo atributo "Activo" es falso
  Cuando intenta iniciar sesión con credenciales correctas
  Entonces el sistema bloquea el acceso con código HTTP 403 Forbidden indicando que la cuenta está inhabilitada.
```

### US-SEG-02: Control de Acceso por Doble Dimensión (Permiso + Unidad Académica)
- **Como** Tech Lead del sistema,
- **Quiero** que el pipeline de autorización evalúe tanto el claim de operación como la unidad académica del recurso,
- **Para** evitar que un administrativo opere sobre propuestas o alumnos de facultades que no tiene asignadas.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Acceso permitido con permiso y unidad coincidente
  Dado que el usuario posee el claim "PROPUESTA.PUBLICAR" y tiene asignada la Unidad Id = 2 (Ciencias Económicas)
  Cuando intenta publicar una versión de propuesta perteneciente a la Unidad Id = 2
  Entonces la autorización es concedida y la operación se ejecuta satisfactoriamente.

Escenario: Acceso denegado por unidad académica no asignada
  Dado que el usuario posee el claim "PROPUESTA.PUBLICAR" pero solo tiene asignada la Unidad Id = 2 (Ciencias Económicas)
  Cuando intenta publicar una propuesta perteneciente a la Unidad Id = 3 (Facultad de Derecho)
  Entonces el sistema rechaza la petición con código HTTP 403 Forbidden.
```

---

## ÉPICA 2: MÓDULO DE PROPUESTAS Y VERSIONES

### US-PROP-01: Creación de Propuesta Madre y Contenido Web
- **Como** administrativo con permiso `PROPUESTA.CREAR_BORRADOR`,
- **Quiero** registrar una nueva propuesta académica madre con su contenido web,
- **Para** incorporarla al catálogo institucional y preparar su posterior publicación.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Alta exitosa de propuesta madre
  Dado que el usuario cuenta con el claim "PROPUESTA.CREAR_BORRADOR" para la Unidad Académica seleccionada
  Cuando envía los datos obligatorios: UnidadId, TipoPropuestaId, Título y datos de contenido web (slug único)
  Entonces el sistema persiste la Propuesta madre en estado Activo y crea su registro asociado PropuestaWeb
  Y retorna el identificador único generado.

Escenario: Intento de registro con slug duplicado
  Dado que ya existe una propuesta web con el slug "diplomatura-ia"
  Cuando un usuario intenta crear otra propuesta con el mismo slug
  Entonces el sistema rechaza la solicitud arrojando un error de validación de unicidad.
```

### US-PROP-02: Creación de Versión / Cohorte en Borrador
- **Como** administrativo con permiso `PROPUESTA.CREAR_BORRADOR`,
- **Quiero** crear una cohorte temporal vinculada a una propuesta madre,
- **Para** definir las fechas de cursado, cupos máximos, aranceles y período lectivo de esa edición.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Creación de cohorte en borrador
  Dado que existe una propuesta madre activa
  Cuando se envía el comando de creación de versión con FechaInicio menor a FechaFin, MaximoAlumnos mayor a 0 y PeriodoLectivoId válido
  Entonces la PropuestaVersion se crea con EstadoPropuestaId = 1 (Borrador)
  Y los cupos disponibles son iguales al MaximoAlumnos ingresado
  Y las inscripciones web quedan deshabilitadas por defecto.

Escenario: Fechas cronológicamente inconsistentes
  Dado el formulario de alta de cohorte
  Cuando se ingresa una FechaInicio mayor o igual a la FechaFin
  Entonces el sistema rechaza la operación con una DomainException explicativa.
```

### US-PROP-03: Publicación y Cierre/Archivado de Versión
- **Como** administrativo con permiso `PROPUESTA.PUBLICAR` / `PROPUESTA.ARCHIVAR`,
- **Quiero** transicionar el estado de la cohorte a Publicada y posteriormente a Archivada,
- **Para** abrir las preinscripciones web y posteriormente cerrarlas para dar paso a la certificación.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Publicación de una versión en borrador
  Dado que la cohorte está en estado Borrador y posee los datos mínimos completos (lugar de realización, aranceles y modalidad)
  Cuando el usuario ejecuta la acción Publicar
  Entonces el estado de la cohorte cambia a "Publicada"
  Y la propuesta se vuelve elegible para admitir preinscripciones web.

Escenario: Transición irreversible a Archivada
  Dado que una cohorte está en estado Publicada
  Cuando el usuario ejecuta la acción Archivar
  Entonces el estado cambia a "Archivada"
  Y se bloquea la recepción de nuevas inscripciones
  Y la versión no puede volver a editarse ni retornar a Borrador o Publicada.
```

---

## ÉPICA 3: MÓDULO DE ADMISIÓN E INSCRIPCIONES

### US-INS-01: Preinscripción Pública de Aspirante desde la Web
- **Como** aspirante interesado en un curso,
- **Quiero** completar el formulario web con mis datos de contacto y documento,
- **Para** solicitar una vacante en una cohorte publicada.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Solicitud de preinscripción exitosa
  Dado que la cohorte está en estado Publicada y permite inscripciones web
  Y existen cupos disponibles (MaximoAlumnos > InscripcionesActivas)
  Cuando el aspirante completa TipoDocumento, NumeroDocumento, Apellido, Nombre y Email válido
  Entonces el sistema registra la Preinscripcion en estado "Pendiente"
  Y emite un UUID único de seguimiento.

Escenario: Cohorte sin cupos disponibles
  Dado que una cohorte alcanzó el cupo máximo de alumnos matriculados
  Cuando un aspirante intenta preinscribirse
  Entonces el sistema bloquea el envío notificando que no hay vacantes disponibles.
```

### US-INS-02: Aprobación de Preinscripción y Matriculación (Atomicidad)
- **Como** administrativo con permiso `INSCRIPCION.APROBAR`,
- **Quiero** validar los datos y confirmar el pago administrativo del aspirante,
- **Para** matricularlo oficialmente como alumno del curso.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Aprobación de nuevo alumno
  Dado que existe una Preinscripcion en estado "Pendiente"
  Y el aspirante no existe previamente en la tabla de Alumnos (según su NumeroDocumento)
  Cuando el administrativo aprueba la preinscripción
  Entonces en una única transacción atómica:
    1. Se crea el Alumno con estado "Activo".
    2. La Preinscripcion pasa a estado "Aprobada" y se le asigna el AlumnoId generado.
    3. Se crea una Inscripcion en estado "Activa" vinculada al AlumnoId y a la PropuestaVersionId.

Escenario: Aprobación de aspirante que ya es alumno existente
  Dado un aspirante con Preinscripción pendiente cuyo NumeroDocumento ya existe en la tabla de Alumnos
  Y su estado es "Activo" o "Inactivo" (sin sanciones disciplinarias)
  Cuando se aprueba la preinscripción
  Entonces se reutiliza el AlumnoId original sin duplicar el registro de Alumno
  Y se crea la nueva Inscripcion activa vinculada.

Escenario: Alumno con sanción disciplinaria vigente
  Dado un aspirante registrado cuyo estado de Alumno es "Bloqueado" o "Suspendido"
  Cuando el administrativo intenta aprobar su preinscripción
  Entonces el sistema aborta la operación arrojando un error de regla de negocio.
```

### US-INS-03: Finalización y Baja de Inscripción
- **Como** administrativo con permiso `INSCRIPCION.CAMBIAR_ESTADO` / `INSCRIPCION.DAR_BAJA`,
- **Quiero** finalizar o dar de baja la cursada de un alumno,
- **Para** reflejar el cierre de ciclo académico o la desvinculación administrativa.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Finalización irreversible de cursada
  Dado que un alumno posee una Inscripcion en estado "Activa"
  Cuando se registra la finalización de cursada
  Entonces la Inscripcion pasa a estado "Finalizada"
  Y esta transición es irreversible (no puede volver a Activa)
  Y el registro queda habilitado para la emisión de su Certificado.

Escenario: Recálculo automático de actividad del alumno
  Dado un alumno cuya única inscripción activa es finalizada o dada de baja
  Cuando se confirma el cambio de estado de la inscripción
  Entonces el estado del Alumno se actualiza automáticamente a "Inactivo".
```

---

## ÉPICA 4: MÓDULO DE CERTIFICADOS

### US-CERT-01: Generación Dinámica de Certificado en PDF
- **Como** administrativo con permiso `CERTIFICADO.GENERAR`,
- **Quiero** generar el certificado digital de un alumno con cursada finalizada,
- **Para** emitir el diploma oficial con sus autoridades firmantes y código QR.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Generación exitosa de certificado
  Dado que la Inscripcion está en estado "Finalizada"
  Y el Alumno está en estado "Activo"
  Y la Unidad Académica cuenta con autoridades con FirmaCertificado = true para el ciclo lectivo
  Cuando se solicita la generación del certificado
  Entonces el sistema calcula el hash criptográfico SHA-256 de los datos
  Y renderiza la plantilla HTML mediante Scriban
  Y genera el archivo PDF mediante PuppeteerSharp incorporando el código QR
  Y persiste el registro en estado "Generado" con Version = 1 y EsVersionActual = true.

Escenario: Bloqueo de emisión por alumno suspendido
  Dado un alumno que completó su cursada pero se encuentra en estado "Suspendido"
  Cuando se intenta emitir su certificado
  Entonces el sistema deniega la emisión arrojando un error de integridad documental.
```

### US-CERT-02: Modificación Controlada por Versionado Histórico
- **Como** administrativo con permiso `CERTIFICADO.MODIFICAR`,
- **Quiero** rectificar un dato textual de un certificado ya emitido (ej. corrección ortográfica del nombre),
- **Para** emitir un documento corregido sin perder la trazabilidad ni borrar la versión previa.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Rectificación de certificado existente
  Dado un Certificado emitido en estado "Generado" con Version = 1
  Cuando el administrativo modifica un dato textual permitido
  Entonces el sistema marca la versión 1 con EsVersionActual = false
  Y genera un nuevo registro con Version = 2, EsVersionActual = true y un nuevo hash SHA-256
  Y el certificado anterior permanece inalterable en la base de datos para auditoría legal.
```

### US-CERT-03: Verificación Pública mediante Código QR
- **Como** tercero validador (empleador o institución),
- **Quiero** escanear el código QR del PDF impreso o digital,
- **Para** verificar instantáneamente en la web de la UCC si el certificado es legítimo y cuál es su estado actual.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Verificación de certificado legítimo
  Dado un certificado escaneado cuyo hash coincide exactamente con la base de datos y su estado es "Generado" o "Aprobado"
  Cuando se accede a la URL pública de validación
  Entonces la página pública muestra un tilde verde confirmando la autenticidad, nombre del alumno, propuesta, horas y fecha de emisión.

Escenario: Certificado revocado o adulterado
  Dado un certificado cuyo estado en el sistema es "Revocado"
  Cuando se escanea el código QR
  Entonces la página pública advierte con un banner rojo que el documento ha sido REVOCADO y carece de validez institucional.
```

---

## ÉPICA 5: MÓDULO PANEL WEB Y LOOKUPS

### US-WEB-01: Catálogo de Solo Lectura para Formularios (Lookups)
- **Como** interfaz web administrativa (`SIGA.WebApp`),
- **Quiero** consultar los catálogos institucionales de Unidades, Tipos de Propuesta, Modalidades y Períodos Lectivos,
- **Para** alimentar los selectores y combos desplegables sin violar la arquitectura ni conectar la web directamente a la base de datos.

#### Criterios de Aceptación (Gherkin):
```gherkin
Escenario: Consulta de lookups de unidades académicas
  Dado un usuario autenticado en el frontend administrativo
  Cuando la aplicación invoca "GET /api/lookups/unidades" (o "GET /api/unidades")
  Entonces el backend responde con una lista JSON de objetos con Id, Nombre, Siglas y Colores Institucionales
  Y el tiempo de respuesta es óptimo al ser una consulta de solo lectura sin tracking de EF Core.
```

---

## ÉPICA 6: FUNCIONALIDADES DIFERIDAS (POST-MVP) `[PENDIENTE]`

### US-FUT-01: Generación de Contenido Web Dinámico con Inteligencia Artificial `[PENDIENTE]`
- **Estado:** `[PENDIENTE - FASE POST-MVP]`
- **Descripción:** Integración con LLM para sugerir automáticamente perfiles de egresado, oportunidades laborales y textos atractivos de marketing basados en el temario del curso cargado por el docente.

### US-FUT-02: Registro y Sellado en Blockchain Federal Argentina (BFA) `[PENDIENTE]`
- **Estado:** `[PENDIENTE - FASE POST-MVP]`
- **Descripción:** Notarización descentralizada del hash SHA-256 del certificado en la red Blockchain BFA para dotar al diploma de inmutabilidad y verificación independiente fuera del servidor de la universidad.

### US-FUT-03: Pasarela de Cobranza Automatizada en Línea `[PENDIENTE]`
- **Estado:** `[PENDIENTE - FASE POST-MVP]`
- **Descripción:** Conexión vía API con pasarela de cobros (Mercado Pago / Plus Pagos) para que el pago en línea apruebe automáticamente la preinscripción e inscriba al aspirante sin intervención manual.

---

## Definition of Done (DoD) General del Proyecto

Una historia de usuario se considerará terminada (**Done**) únicamente cuando cumpla con:
1. **Compilación Limpia:** El código compila sin errores ni advertencias severas en .NET 10.
2. **Invariantes Protegidos:** Las validaciones de negocio del dominio están implementadas en entidades y casos de uso con `DomainException`.
3. **Seguridad Aplicada:** Los endpoints correspondientes exigen autenticación JWT y validan los Claims y la Unidad Académica del usuario operador.
4. **Mapeo Tipado:** Los DTOs se transforman hacia/desde entidades mediante métodos generados por `Riok.Mapperly`.
5. **Persistencia y Transacciones:** Las operaciones críticas (como aprobación de preinscripción o versionado de certificados) se ejecutan dentro de transacciones atómicas de base de datos.
6. **Contrato OpenAPI:** El endpoint está correctamente documentado en Swagger con sus códigos de respuesta esperados (200, 400, 401, 403, 404, 500).
