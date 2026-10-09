## Preinscripciones

El sistema permite la gestión de **Preinscripciones** de aspirantes a través del portal web institucional. Esta entidad actúa como el disparador del proceso de admisión y regula la transición del aspirante hacia la formalización de su inscripción.

El estado de una preinscripción determina las acciones permitidas y su relación con otras entidades del sistema:

1. **Pendiente:** Toda preinscripción creada desde la página web asume este estado por defecto. En esta etapa, el registro es preventivo y no permite la vinculación de una **Inscripción** formal.

2. **Aprobada:** Tras la validación de un usuario administrativo, la preinscripción cambia a este estado. Este cambio es el **único habilitador** para que el registro se asocie a una **Inscripción**.

3. **Revocada:** Un administrativo puede dar por finalizado el proceso negando la solicitud. Una preinscripción en este estado queda inhabilitada para generar inscripciones.

Para asegurar la integridad de los datos, se deben aplicar las siguientes validaciones:

* **Validación de Identidad Obligatoria:** No se puede transicionar una preinscripción al estado **"Aprobada"** si el aspirante no está registrado previamente como **Alumno** en el sistema. El administrativo deberá crear o vincular al alumno antes de proceder con la aprobación.

* **Restricción de Asociación:** Solo las preinscripciones con estado **"Aprobada"** pueden tener una **Inscripción** asociada. El sistema debe bloquear cualquier intento de crear una inscripción si el estado es "Pendiente" o "Revocada".

* **Integridad de Datos:** La Preinscripción debe mantener integridad referencial con las entidades de **Alumno**, **Propuesta Académica** y **Estado de Preinscripción**.

* **Disparadores (Triggers):** Al confirmar la transición al estado **"Aprobada"**, el sistema debe:
    1. Vincular permanentemente el `AlumnoId` al registro de Preinscripción.
    2. Ejecutar la migración de datos hacia la entidad **Inscripciones**.
    3. Notificar automáticamente al aspirante sobre su nueva condición.

---

## Alumnos

La entidad **Alumno** representa al individuo dentro del ecosistema académico. Su existencia es única y centralizada, permitiendo que un mismo sujeto posea múltiples trayectorias académicas a través de diversas inscripciones.

Cuando una **Preinscripción** transiciona al estado **"Aprobada"**, el sistema debe ejecutar un servicio de automatización de identidad bajo las siguientes reglas:

1. **Verificación de Identidad:** El sistema realizará una búsqueda mandatoria por **DNI** (o documento equivalente).
* **Si el Alumno no existe:** Se procede a la creación del registro maestro de Alumno con los datos provistos en la preinscripción.
* **Si el Alumno ya existe:** Se reutiliza el registro existente, evitando la duplicidad de datos.

2. **Generación de Inscripción:** Independientemente de si el alumno es nuevo o existente, el servicio creará una nueva entidad **Inscripción** vinculando el `AlumnoId` con la `PropuestaId` seleccionada.

3. **Relación Uno a Muchos:** El sistema debe garantizar que un Alumno pueda estar asociado a múltiples Inscripciones, pero un Alumno nunca debe duplicarse en la base de datos.

El estado del alumno regula su capacidad operativa dentro de la institución y depende de su situación académica o administrativa:

* **Activo:** El alumno posee al menos una inscripción vigente. Está habilitado para inscribirse a nuevas propuestas, solicitar certificados y participar en actividades institucionales.
* **Inactivo:** El alumno no posee inscripciones vigentes (por egreso, baja voluntaria o falta de actividad). No posee sanciones, pero tiene restringido el acceso a beneficios de alumno regular hasta que inicie un nuevo proceso.
* **Suspendido:** Estado de penalización temporal (ej. 6 meses). El alumno mantiene su historial, pero tiene bloqueada la creación de nuevas inscripciones y el cursado por un periodo determinado. Puede retornar al estado **Activo** de forma automática (por fecha) o manual.
* **Bloqueado:** Estado de veto permanente o por decisión administrativa grave (fraude, documentación apócrifa). El alumno pierde todo derecho a trámites, certificados o nuevas inscripciones.

Para mantener el control sobre la matrícula, se aplican las siguientes restricciones de estado:

* **Persistencia de Sanciones:** Un alumno en estado **Bloqueado** o **Suspendido** no puede transicionar a otros estados de forma automática por el sistema (por ejemplo, al aprobar una nueva preinscripción). Cualquier cambio de estos estados debe ser auditado y ejecutado por un usuario administrativo de alto rango.
* **Cálculo Automático de Actividad:** El sistema debe verificar periódicamente el estado de las inscripciones asociadas. Si un alumno cierra su última inscripción activa, su estado debe virar automáticamente a **Inactivo**.
* **Restricción de Egreso:** No se podrán emitir certificados analíticos finales o títulos si el alumno se encuentra en estado **Suspendido** o **Bloqueado**.

---

## Inscripciones

La entidad **Inscripción** formaliza el vínculo entre un **Alumno** y una **Propuesta Académica**. Es el resultado directo de una **Preinscripción Aprobada** y representa la trayectoria activa o histórica del estudiante en la institución.

El estado de una inscripción es determinante para la condición de actividad del alumno:

1. **Activa:** Representa el cursado vigente. Se genera automáticamente al aprobarse la preinscripción.
2. **Finalizada:** Estado de cierre de ciclo (por aprobación total o cumplimiento de términos). Este estado es **irreversible**.
3. **Baja:** Cierre prematuro por solicitud del alumno o decisión administrativa. Invalida la cursada actual.

Para garantizar la consistencia del modelo académico, se deben cumplir las siguientes restricciones:

* **Dependencia de Preinscripción:** Toda **Inscripción** debe poseer una relación obligatoria  con una **Preinscripción** en estado **"Aprobada"**. No existen inscripciones huérfanas o creadas manualmente sin este disparador.
* **Unicidad y Restricción de Duplicidad:** El sistema debe impedir que un alumno se inscriba a una propuesta en la que ya posee una inscripción vigente.
* **Bloqueo:** No se puede crear una inscripción si el alumno ya tiene una inscripción **Activa** o **Finalizada** para la misma propuesta.
* **Excepción:** Solo se permitirá una nueva inscripción a la misma propuesta si la anterior posee estado **Baja**.

* **Gestión de Finalización:** La transición al estado **Finalizada** puede ser ejecutada por un administrativo o mediante un proceso programado. Una vez en este estado, la inscripción no puede retornar a **Activa**.
* **Impacto en el Alumno:** Al cambiar una inscripción a **Finalizada** o **Baja**, el sistema debe verificar el resto de las inscripciones del alumno. Si no cuenta con ninguna otra en estado **Activa**, el **Alumno** pasará automáticamente a **Inactivo**.

Para asegurar la robustez del servicio de conversión, se deben contemplar los siguientes escenarios críticos:

1. Conflicto de Identidad y Sanciones Previas

    Si al intentar crear la inscripción el sistema detecta mediante el DNI que el Alumno ya existe y posee un estado de **Bloqueado** o **Suspendido**:

        * **Acción:** El sistema debe abortar el proceso de inscripción.
        * **Regla:** Un alumno con sanciones vigentes no puede generar nuevas inscripciones aunque su preinscripción web haya sido aprobada inicialmente.

2. Atomicidad del Proceso de Conversión

Existe el riesgo de que la preinscripción cambie a "Aprobada" pero el servicio de creación de Alumno o Inscripción falle.

    * **Acción:** La actualización del estado de la Preinscripción, la búsqueda/creación del Alumno y el alta de la Inscripción deben ejecutarse dentro de una **transacción única**. Si falla un paso, se debe realizar un *rollback* completo.

3. Validación de Vigencia Temporal

    * **Regla:** El sistema debe impedir que la `Fecha de Finalización` (programada o manual) sea cronológicamente anterior a la `Fecha de Alta` de la inscripción.

 4. Reintentos de Inscripción post-Baja

    * **Escenario:** Un alumno con una inscripción previa en estado **Baja** realiza una nueva preinscripción.
    * **Regla:** El sistema debe permitir la nueva inscripción pero está estrictamente prohibido crear un nuevo registro de Alumno. Se debe reutilizar el `AlumnoId` original para mantener la trazabilidad histórica del sujeto.



3. Validación de Duplicidad (Nivel Servicio de Aplicación)
Aunque la entidad Inscripcion gestiona su estado, la regla de "No se puede inscribir si ya tiene una Activa o Finalizada" debe validarse en el servicio que coordina la inscripción (Service Layer), consultando el repositorio:

Regla Pro: "No permitas CrearDesdePreinscripcion si repo.Any(x => x.AlumnoId == id && x.PropuestaId == pid && x.Estado != Baja)".



---



## Autoridades

La entidad **Autoridad** gestiona la información de los funcionarios habilitados para la validación y firma de documentos oficiales. Su validez está estrictamente vinculada a la estructura organizativa y temporal de la institución.

### Configuración y Vinculación Jerárquica

Para que una autoridad sea asociada correctamente a un documento (Certificado), el sistema sigue una cadena de integridad referencial basada en la **Unidad Académica**:

1. **Relación con la Unidad Académica:** Toda autoridad debe estar asociada obligatoriamente a una Unidad Académica desde su creación.
2. **Trazabilidad del Certificado:** La determinación de las firmas se realiza mediante el siguiente flujo de resolución:
* `Certificado`  vinculado a una `Inscripción`.
* `Inscripción`  vinculada a una `Propuesta`.
* `Propuesta`  pertenece a una `Unidad Académica`.
* `Unidad Académica`  determina el pool de `Autoridades` disponibles.


3. **Consistencia Temporal:** La autoridad debe estar vinculada al **Ciclo Lectivo** correspondiente a la propuesta académica. El sistema solo considerará como firmantes a aquellas autoridades cuyo ciclo lectivo coincida con el del proceso del alumno.

Reglas de Negocio para Firmas

* **Atributo de Firma:** Una autoridad solo aparecerá como firmante en los certificados si su atributo `firmaCertificado` está definido como **True**.
* **Orden de Prelación:** En la sección de parametrización, se puede definir el **Orden de la Firma**, determinando la posición jerárquica en la que aparecerán las rúbricas en el documento final.

Restricciones de Edición e Integridad

Para proteger la validez legal de los documentos ya emitidos, se aplican las siguientes restricciones de modificación:

* **Bloqueo de Ciclo Lectivo:** Está estrictamente prohibido editar el **Ciclo Lectivo** de una autoridad si ya existen certificados generados y vinculados a dicho registro.
* **Modificaciones Permitidas:** En caso de existir vínculos activos, el administrador solo podrá realizar cambios cosméticos o de forma, tales como:
1. Nombre de la autoridad.
2. Orden de la firma en la parametrización.


* **Inalterabilidad Histórica:** Cualquier cambio de autoridad para un nuevo periodo debe gestionarse mediante la creación de un nuevo registro o vinculación, preservando los datos de las autoridades anteriores para consultas históricas.

---

#### 1. Vacancia de Firma

* **Escenario:** Se intenta generar un certificado para una propuesta cuya Unidad Académica no tiene autoridades con `firmaCertificado = True` para el ciclo lectivo vigente.
* **Acción:** El sistema debe emitir una excepción y bloquear la generación del certificado para evitar documentos sin validez legal.

#### 2. Superposición de Autoridades

* **Escenario:** Existencia de múltiples autoridades con el mismo rango u orden de firma para una misma Unidad Académica y Ciclo Lectivo.
* **Regla:** El sistema debe validar que no existan conflictos de jerarquía en la parametrización antes de permitir la activación del atributo de firma.

#### 3. Desfase de Ciclos Lectivos

* **Riesgo:** Una propuesta que abarca más de un ciclo lectivo.
* **Validación:** El sistema debe tomar la autoridad vigente al momento de la **emisión** del certificado, o según la configuración específica de la Propuesta Académica si requiere la autoridad del año de cursada.

---


## Certificados

La entidad **Certificado** es el documento oficial que acredita la culminación de una trayectoria académica. Su generación es un proceso derivado que consolida la información del alumno, la propuesta y las autoridades vigentes.

### Requisitos de Emisión

La generación de un certificado está sujeta a una validación estricta de estados tanto en la inscripción como en el registro del alumno:

1. **Estado de la Inscripción:** Únicamente las inscripciones en estado **"Finalizada"** son elegibles para la emisión de certificados.
* **Bloqueo de Seguridad:** El sistema debe impedir la creación de certificados para inscripciones en estado **"Activa"** o **"Baja"**.


2. **Condición del Alumno:** Se realiza una verificación de integridad administrativa sobre el Alumno.
* **Restricción por Sanción:** Si el alumno se encuentra en estado **"Bloqueado"** o **"Suspendido"**, el sistema debe inhabilitar la generación de cualquier certificado hasta que su situación sea regularizada.



### Composición y Plantillas

El certificado se comporta como una entidad dinámica basada en plantillas institucionales:

* **Inalterabilidad de la Propuesta:** Una vez iniciado el proceso de certificación, la **Propuesta Académica** asociada es inmutable. No se permite el cambio de propuesta dentro de un mismo certificado.
* **Automatización de Datos:** El sistema utiliza la información del **Alumno** y la **Propuesta** para completar automáticamente una plantilla predefinida.
* **Integración de Firmas:** El documento recupera las **Autoridades** vinculadas a la Unidad Académica y Ciclo Lectivo correspondientes, posicionándolas según su **Orden de Firma** y siempre que tengan activo el permiso de firma.

### Gestión de Cambios e Historial

Para asegurar la trazabilidad y la seguridad documental, el sistema implementa las siguientes reglas de persistencia:

* **Edición Controlada:** Se permite la edición de información textual dentro del certificado, pero cualquier modificación dispara un evento de versionado.
* **Historial de Cambios:** Cada vez que un certificado es generado o modificado, el sistema debe almacenar una copia del registro anterior. Esto permite mantener una auditoría completa de las versiones previas antes de la última modificación.
* **Salida Digital:** El resultado final de este proceso es la generación y guardado de un archivo en formato **PDF**, el cual debe ser consistente con la última versión registrada en el historial.

---

### Validación de Irregularidades y Casos de Borde

#### 1. Desincronización de Datos del Alumno

* **Escenario:** El alumno cambia su nombre o DNI (por rectificación legal) después de que se generó un certificado.
* **Regla:** El sistema debe permitir la regeneración del certificado para reflejar los datos actuales del alumno, almacenando la versión anterior en el historial para auditoría.

#### 2. Ausencia de Autoridades para el Ciclo

* **Riesgo:** Una inscripción finaliza en un Ciclo Lectivo que no tiene autoridades parametrizadas.
* **Acción:** El sistema debe notificar al administrador la ausencia de autoridades firmantes antes de permitir la generación del PDF.

#### 3. Intentos de Fraude por Cambio de Estado

* **Situación:** Se intenta emitir un certificado y, en el milisegundo previo, el alumno es **Bloqueado**.
* **Solución:** El servicio de generación de PDF debe realizar una validación de estado del Alumno *justo antes* de la firma digital o guardado del archivo (validación *just-in-time*).

---


Aquí tienes la redacción para el módulo de **Propuestas**, siguiendo la estructura de reglas de negocio, ciclo de vida y casos de borde que venimos utilizando para tu sistema.

---

## Propuestas

La entidad **Propuesta** define la oferta académica (cursos, jornadas, carreras) de la institución. Su estado determina no solo su visibilidad pública, sino también la integridad de los documentos legales que de ella dependan.

### Ciclo de Vida de la Propuesta

El flujo de estados es lineal y restrictivo para asegurar la consistencia de los datos históricos:

1. **Borrador:** Es el estado inicial de creación. En esta etapa, el registro es totalmente editable y es el **único estado** que permite la eliminación física del registro en el sistema.
2. **Publicada:** La propuesta es oficializada por un administrativo.
* **Admisión:** Solo en este estado se puede activar o desactivar la recepción de **Preinscripciones** y la visibilidad en el portal web institucional.
* **Restricción de Cambio:** Una propuesta publicada puede volver a editarse bajo limitaciones estrictas, pero no puede retornar al estado Borrador.


3. **Archivada:** Cierre definitivo de la propuesta. Una vez archivada, la propuesta **no puede volver a publicarse** ni permite nuevas inscripciones o visualización web. Se mantiene en el sistema exclusivamente para fines históricos y de integridad referencial.

### Reglas de Negocio y Edición

Para proteger la validez de los certificados ya emitidos o en proceso, se aplican las siguientes reglas:

* **Inmutabilidad del Título:** Una vez que la propuesta sale del estado "Borrador", el **Título** no puede ser actualizado bajo ninguna circunstancia, ya que este campo se utiliza de forma literal para la generación de certificados.
* **Modificación Restringida:** * En **Borrador**, la edición es total.
* En **Publicada**, solo se pueden editar propiedades accesorias (descripciones, fechas de cursado, imágenes), excluyendo identificadores críticos y el título.
* En **Archivada**, la edición está totalmente bloqueada; el registro se vuelve de "Solo Lectura".


* **Control de Publicación:** La capacidad de admitir inscripciones es un interruptor (*toggle*) que solo reside dentro del estado **Publicada**. Si la propuesta cambia de estado, cualquier proceso de inscripción activo debe ser interrumpido por el sistema.

---

### Validación de Irregularidades y Casos de Borde

#### 1. Intento de Eliminación de Propuestas con Historial

* **Escenario:** Un usuario intenta eliminar una propuesta que ya fue publicada o que tiene alumnos preinscriptos.
* **Regla:** El sistema debe bloquear la eliminación. Solo se permite el borrado físico si el estado es **Borrador** y no existen registros dependientes (Integridad Referencial).

#### 2. Cambio de Título Crítico

* **Riesgo:** Un administrativo necesita corregir una errata en el título de una propuesta ya publicada.
* **Solución:** Al estar bloqueado el título por seguridad documental, la solución administrativa debe ser archivar la propuesta errónea y crear una nueva desde cero (Borrador), asegurando que los certificados anteriores no se vean afectados por el cambio.

#### 3. Persistencia de la Página Web

* **Escenario:** Una propuesta pasa de **Publicada** a **Archivada** mientras un aspirante tiene abierta la página de inscripción.
* **Acción:** El servicio web debe validar el estado de la propuesta en cada interacción. Al detectar el cambio a "Archivada", debe redirigir al usuario o mostrar un mensaje de "Propuesta no disponible", bloqueando el envío del formulario de preinscripción.


