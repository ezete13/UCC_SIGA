# 01 — Glosario del Dominio UCC_SIGA

Este glosario define los términos unívocos y oficiales del lenguaje ubicuo del dominio de UCC_SIGA, respaldados estrictamente por las decisiones tomadas en la auditoría y los requerimientos funcionales.

---

## 1. Estructura Organizativa y Catálogos

- **Unidad Académica (Unidad):** Dependencia académica institucional (Facultad, Escuela o Instituto; ej. *Facultad de Ciencias Económicas*, *Facultad de Derecho*). Las propuestas, autoridades y usuarios están asociados a una o varias unidades. Es administrada exclusivamente por base de datos (DBA).
- **Tipo de Propuesta:** Clasificación académica formal de la oferta (ej. *Curso*, *Diplomatura*, *Jornada*, *Taller*, *Seminario*).
- **Período Lectivo (Ciclo Lectivo):** Ventana temporal anual o semestral formalmente delimitada por fechas de inicio y fin, que enmarca el dictado de las cohortes y la vigencia reglamentaria de las autoridades firmantes.
- **Modalidad:** Mecanismo pedagógico de impartición de la propuesta (*Presencial*, *Virtual*, *Híbrida*).

---

## 2. Oferta Académica y Cohortes

- **Propuesta (Propuesta Madre):** Registro catálogo que define la oferta académica permanente o de largo plazo. Contiene el título institucional, la unidad académica responsable, la fundamentación y el contenido web. Su estado es permanente y activo.
- **PropuestaWeb (Contenido Web / Landing Page):** Sección y metadatos de difusión pública vinculados 1:1 con la Propuesta madre. Incluye descripción atractiva, perfil del estudiante, requisitos de admisión, banners, etiquetas SEO y un `slug` único para navegación amigable en la web pública.
- **PropuestaVersion (Versión / Cohorte):** Instancia operativa y temporal concreta en la que se dicta una propuesta madre. Define un período lectivo, una modalidad de cursado, fechas límites, aranceles, cupos máximos y cuerpo docente asignado.
- **Estados de la Propuesta / Versión:**
  - `Borrador`: Estado inicial de carga de la versión. Totalmente editable y eliminable físicamente si no tiene dependencias. No admite inscripciones ni visibilidad web.
  - `Publicada`: Versión oficializada. Habilita visibilidad web y apertura de recepción de preinscripciones. No puede eliminarse ni retornar al estado Borrador.
  - `Archivada`: Versión cuyo ciclo lectivo o período de admisión concluyó. Se desactiva la recepción de inscripciones y la edición queda bloqueada. Sus inscripciones finalizadas quedan habilitadas para certificación.
- **Temario:** Detalle modular del plan de estudios vinculado a la Propuesta madre, compuesto por módulos ordenados, contenidos temáticos y asignación horaria.
- **Docente:** Profesional académico registrado en el sistema con su identidad, número de documento y perfil profesional.
- **PropuestaVersionDocente:** Asignación de un docente a una cohorte específica de una propuesta, especificando su rol (*Titular*, *Adjunto*, *Invitado*) y orden de visibilidad en el sitio web.

---

## 3. Admisión y Matrícula

- **Aspirante:** Persona interesada que envía sus datos a través de una página web pública para solicitar un lugar en una versión específica de una propuesta.
- **Preinscripción:** Registro preventivo y provisional originado desde la web pública o cargado por un operador. Representa la postulación del aspirante antes de su admisión formal.
- **Estados de Preinscripción:**
  - `Pendiente`: Estado por defecto. El registro aguarda revisión administrativa y validación de pago/requisitos.
  - `Aprobada`: Validación positiva por parte de un administrativo. Es el disparador exclusivo que genera o vincula al Alumno y da de alta la Inscripción formal.
  - `Revocada`: Solicitud rechazada o descartada administrativamente. No habilita matriculación.
- **Alumno:** Registro maestro único y centralizado de la persona física dentro del ecosistema universitario. Un mismo individuo posee un único registro de alumno (identificado unívocamente por su tipo y número de documento) sin duplicaciones, pero puede poseer múltiples trayectorias académicas a través de sus inscripciones.
- **Estados de Alumno:**
  - `Activo`: Posee al menos una inscripción vigente en el sistema. Habilitado para cursar, tramitar y certificar.
  - `Inactivo`: No posee inscripciones vigentes (por cursadas concluidas o bajas).
  - `Suspendido`: Sanción disciplinaria temporal. Bloquea nuevas inscripciones y emisión de certificaciones.
  - `Bloqueado`: Veto institucional definitivo por faltas graves o fraude documental. Bloqueo total de trámites.
- **Inscripción:** Formalización reglamentaria del vínculo académico entre un Alumno y una `PropuestaVersion`.
- **Estados de Inscripción:**
  - `Activa`: Cursado en curso.
  - `Finalizada`: Ciclo de cursado completado satisfactoriamente por el alumno. Transición irreversible que habilita la emisión de certificados.
  - `Baja`: Interrupción anticipada de la cursada por renuncia del alumno o decisión administrativa.
- **Cupos Disponibles:** Valor numérico derivado en tiempo real:
  $$\text{CuposDisponibles} = \text{MaximoAlumnos} - \text{InscripcionesActivas}$$
  No se persiste como columna física para evitar inconsistencias por concurrencia.

---

## 4. Documentación y Certificación

- **Certificado:** Documento oficial digital que acredita la aprobación o participación de un alumno en una cohorte académica.
- **Estados del Certificado:**
  - `No Generable`: La inscripción aún no cumple las condiciones reglamentarias (no está finalizada o el alumno está suspendido).
  - `Pendiente`: Se cumplen todas las condiciones requeridas, pero el documento aún no ha sido emitido.
  - `Generado`: El certificado ha sido emitido y almacenado en el sistema con su correspondiente identificador y hash criptográfico. Permite correcciones únicamente mediante versionado.
  - `Aprobado`: Certificado validado institucionalmente. Oficial y definitivo, bloqueado contra nuevas versiones ordinarias.
  - `Inhabilitado`: Suspensión transitoria de la validez del certificado por revisión administrativa. El QR refleja su invalidez.
  - `Revocado`: Invalidación legal permanente y definitiva. El QR refleja que el certificado ha sido revocado.
- **Versionado de Certificado:** Mecanismo por el cual cualquier modificación a los datos textuales de un certificado ya emitido no sobreescribe el registro existente, sino que genera una nueva versión (`Version = Version + 1`, `EsVersionActual = true`), marcando la versión previa como histórica para auditoría legal.
- **Hash de Seguridad:** Cadena hexadecimal de 64 caracteres generada mediante el algoritmo criptográfico SHA-256 a partir de los datos inmutables del certificado (identificador, alumno, propuesta, fecha y token).
- **Código QR de Verificación:** Código bidimensional embebido en el PDF del certificado que dirige a una URL pública de consulta institucional para validar la autenticidad e integridad del documento emitido.
- **Autoridad:** Funcionario universitario habilitado para suscribir certificados en representación de una Unidad Académica durante un Ciclo Lectivo determinado.
- **Firma Certificado:** Indicador booleano que determina si la autoridad debe figurar activamente como rúbrica en los certificados emitidos.
- **Orden de Firma:** Prioridad jerárquica numérica que define la posición de izquierda a derecha de las firmas en el pie del documento.

---

## 5. Seguridad y Gobernanza

- **Permiso (Claim):** Constante técnica que autoriza una acción u operación atómica sobre un módulo del sistema (ej. `PROPUESTA.PUBLICAR`, `CERTIFICADO.GENERAR`). Se asignan a roles y los roles a usuarios.
- **Control de Acceso de Doble Dimensión:** Regla de autorización obligatoria en runtime donde no basta con tener el permiso funcional; la operación solo es válida si el recurso intervenido pertenece a una de las **Unidades Académicas** explícitamente asignadas al usuario.
