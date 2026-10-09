La idea es crear una aplicación web para gestionar las propuestas que ofrece una universidad que tenga los siguientes módulos:

1. Módulo de Seguridad
El Módulo de Seguridad gestiona la autenticación y autorización del personal interno de la universidad. Permite administrar usuarios, roles y permisos, garantizando que cada usuario acceda únicamente a los módulos, operaciones y unidades académicas para las que se encuentre autorizado. El sistema implementa un modelo de control de acceso basado en permisos, donde no todos los usuarios pueden acceder a todos los módulos, ni realizar todas las operaciones dentro de un mismo módulo. Asimismo, la universidad se encuentra organizada en unidades académicas (por ejemplo: Unidad de Ciencias Económicas, Unidad de Filosofía, etc.), y los accesos pueden restringirse a una o varias de estas unidades. El control de acceso se aplica en dos niveles:

Por módulo y operación:
 Define qué acciones puede realizar un usuario dentro de cada módulo (ver, crear, modificar, publicar, aprobar, etc.).

Por unidad académica:
 Define sobre qué unidades académicas puede operar el usuario. Por ejemplo, un usuario puede tener acceso al módulo de Inscripciones, pero únicamente visualizar inscripciones correspondientes a la Unidad de Ciencias Económicas. Ambos niveles se combinan para determinar el acceso efectivo del usuario dentro del sistema.

Funcionalidades del módulo
- Crear, editar y desactivar usuarios.
- Crear, editar y desactivar roles.
- Asignar permisos a los roles.
- Asignar una o varias unidades académicas a cada usuario.
- Controlar el acceso a módulos y operaciones según los permisos asignados.

Modelo de permisos (Claims)
El sistema utiliza un esquema de permisos definidos mediante claims, implementados a nivel de entidad en la plataforma .NET.
 Cada claim representa una acción específica sobre un módulo determinado y es utilizado para validar el acceso del usuario en tiempo de ejecución.
Permisos por módulo

Permisos Modulo de Seguridad
USUARIO.CREAR
USUARIO.EDITAR
USUARIO.DESACTIVAR
ROL.CREAR
ROL.EDITAR
ROL.DESACTIVAR
PERMISO.ASIGNAR
UNIDAD.ASIGNAR


Persmisos Modulo Propuestas
PROPUESTA.VER
PROPUESTA.CREAR_BORRADOR
PROPUESTA.PUBLICAR
PROPUESTA.ARCHIVAR

Inscripciones
INSCRIPCION.VER
INSCRIPCION.APROBAR
INSCRIPCION.DAR_BAJA
INSCRIPCION.CAMBIAR_ESTADO

Certificados
CERTIFICADO.VER
CERTIFICADO.GENERAR
CERTIFICADO.MODIFICAR
CERTIFICADO.REVOCAR
CERTIFICADO.DESCARGAR_PDF

Web
WEB.PUBLICAR
WEB.MODIFICAR
WEB.DESACTIVAR


Consideraciones finales
- Los permisos se asignan a roles, y los roles a usuarios.
- Los accesos siempre se validan considerando:
- los permisos asignados (claims), y las unidades académicas habilitadas para el usuario.
- Este modelo permite una administración flexible, escalable y alineada con las necesidades organizativas de la universidad.


=========================================================== 

2. Módulo de Propuestas
El Módulo de Propuestas permite administrar las carreras, cursos y jornadas que ofrece la universidad. Desde este módulo se crean y gestionan las propuestas que luego se publican en el sitio web institucional para que los alumnos puedan inscribirse. Además, las propuestas constituyen la base para la emisión de los certificados que se entregan a los participantes.
Cada propuesta atraviesa distintos estados, los cuales determinan qué acciones pueden realizarse sobre ella y su visibilidad en la web. Comprender estos estados es fundamental para el correcto uso del sistema.

Borrador: 
- Es el estado inicial de todas las propuestas nuevas.
- La página web pública no puede activarse.
- No se permiten inscripciones.
- La propuesta puede ser modificada o eliminada por el rol correspondiente.
 - Desde este estado, la propuesta puede avanzar a los estados siguientes.

Publicado:
 - Este estado formaliza la propuesta dentro del sistema.
- Se habilita la activación de la página web pública.
- Se permiten nuevas inscripciones.
- Solo algunos campos pueden ser modificados.
- La propuesta no puede ser eliminada.
- No es posible volver al estado Borrador.

Archivado:
 - Son propuestas que estuvieron publicadas y finalizaron su período de inscripción.
- No se admiten nuevas inscripciones.
- La propuesta no es visible en la web pública.
- La página web se desactiva automáticamente.
- No se permite su modificación ni eliminación.
- Únicamente las propuestas archivadas pueden utilizarse para la emisión de certificados.


===========================================================

3. Módulo de Inscripciones
El Módulo de Inscripciones permite gestionar las inscripciones de los alumnos a las propuestas (carreras, cursos o jornadas). Desde este panel es posible:
Registrar nuevas inscripciones de manera manual, visualizar las inscripciones recibidas desde las páginas web públicas, gestionar el estado de cada alumno a lo largo del proceso de inscripción, administrar cupos, bajas y finalización de cursadas. Cada inscripción atraviesa diferentes estados que reflejan la situación del alumno dentro del curso o propuesta.

Solicitante:
 - Corresponde al alumno que completa y envía el formulario de inscripción desde la web pública.
 - Es el estado inicial de toda inscripción.
- La inscripción se registra automáticamente en el sistema.
- Se descuenta un cupo del total disponible para la propuesta.
- Los datos ingresados aún no han sido validados por la universidad.

Preinscripto:
 - Estado en el cual la inscripción se encuentra en revisión.
 - El personal administrativo puede verificar que los datos del alumno sean correctos y completos.
- En caso de inconsistencias, puede contactarse al alumno para solicitar correcciones o información adicional.
- La inscripción aún no está confirmada de manera definitiva.

Inscripto:
 - Indica que la inscripción ha sido aceptada por la universidad.
- El alumno ya forma parte oficialmente del curso.
- Este estado puede alcanzarse, según la política de la universidad, luego de la confirmación administrativa, la generación de una deuda, o la acreditación de un pago.

Baja:
 - Representa a un alumno que deja de formar parte del curso.
- Una vez en este estado, el alumno no continúa participando del curso. 
 - Este estado puede aplicarse por distintos motivos, como decisión del alumno, incumplimiento de requisitos, razones administrativas.

Finalizado:
 -Corresponde a los alumnos que completaron el curso.
-El alumno se encuentra en condiciones de recibir su certificado, siempre que la propuesta esté archivada.
-La inscripción queda cerrada a futuras modificaciones operativas.


=========================================================== 

4. Módulo de Certificados
El Módulo de Certificados permite gestionar la emisión y administración de los certificados correspondientes a las propuestas académicas. Este módulo muestra únicamente a los alumnos que se encuentran en condiciones de recibir un certificado, considerando que las inscripciones estén en estado Finalizado y las propuestas en estado Archivado. Los certificados pueden ser modificados únicamente mediante un sistema de versionado. Cada cambio genera una nueva versión del certificado, manteniendo el historial completo de modificaciones y preservando las versiones anteriores para auditoría. La eliminación de certificados no está permitida. Los certificados pueden emitirse en formato PDF de manera dinámica, generándose automáticamente a partir de los datos almacenados.
Cada certificado incluye un código QR que permite verificar su autenticidad.
 Al escanearlo, se podrá visualizar el estado actual del certificado (por ejemplo, válido, inhabilitado o revocado). Al mismo tiempo cada certificado atraviesa distintos estados que determinan su nivel de edición, generación y validez institucional:

No Generable:
 - Estado inicial del certificado.
 - Indica que el alumno aún no cumple con todos los requisitos necesarios para la emisión del certificado
- El certificado no puede generarse.
- No puede editarse ni eliminarse.

Pendiente:
 - El alumno cumple con todas las condiciones para recibir el certificado.
- El certificado puede ser generado.
- Aún no existe un certificado emitido en la base de datos.
- No permite edición, ya que el certificado aún no ha sido creado.

Generado:
 - El certificado ha sido emitido y registrado en el sistema.
- Puede ser modificado únicamente mediante versionado.
- No puede eliminarse.
- El PDF puede emitirse nuevamente respetando la versión vigente.

Aprobado:
 - El certificado ha sido validado institucionalmente.
- No puede ser modificado.
- No puede eliminarse.
- Se considera plenamente válido y oficial.

Inhabilitado:
 - El certificado pierde su validez de forma temporal o preventiva.
 - Puede utilizarse, por ejemplo, en casos de revisión administrativa o inconsistencias detectadas.
- No puede ser editado ni eliminado.
- El QR refleja su condición de certificado no válido.

Revocado:
 - El certificado ha sido invalidado de forma definitiva.
 -Se utiliza en casos excepcionales, como errores graves o situaciones administrativas justificadas.
-El certificado no puede ser modificado ni eliminado.
- El QR indica que el certificado fue revocado y no es válido.


=========================================================== 

5. Módulo Panel Web
El Panel Web permite gestionar el contenido y la visibilidad de las páginas web públicas asociadas a las propuestas académicas. Desde este módulo es posible administrar la información que se muestra en la web, controlar la publicación de cada página y realizar un seguimiento de sus visitas.
Este módulo se encuentra directamente vinculado con el Módulo de Propuestas, ya que cada propuesta cuenta con una sección web propia que puede ser activada o desactivada de forma independiente.

- Activar o desactivar la página web de una propuesta
 Permite publicar o retirar de la web pública la página correspondiente a una propuesta.
- Activar o desactivar el formulario de inscripción
 Controla la disponibilidad del formulario para que los alumnos puedan inscribirse desde la web.
- Gestionar el contenido web de la propuesta
 Permite cargar y modificar la información relevante que se muestra en la página pública, como descripciones, fechas, requisitos y otros datos informativos.
- Validación de contenidos obligatorios
 Para garantizar un estándar común en todas las páginas web, el sistema exige que determinados campos obligatorios estén completos.
 Si esta información no se encuentra cargada, la activación de la página web de la propuesta no será posible.
- Control de visitas
 Permite visualizar métricas básicas de visitas a la página web de cada propuesta.

=========================================================== 

Implementaciones Futuras
Panel Web
 En una etapa futura, se prevé incorporar contenido dinámico generado mediante inteligencia artificial. Este contenido podrá adaptarse según cada visita, permitiendo mostrar información personalizada, como secciones informativas actualizadas sobre oportunidades laborales u otros datos relevantes, manteniendo siempre un alcance acotado y controlado.

Panel de Certificados
 En una fase posterior, se evaluará la integración del registro de certificados en la BFA (Blockchain Federal Argentina), con el objetivo de reforzar la autenticidad, trazabilidad y validación institucional de los certificados emitidos.










admin/propuestas/

/Inicio -> HTML del inicio del módulo aquí se muestran 3 secciones o tablas que muestran las propuestas en borrador, las propuestas publicadas y las propuestas archivadas. También se muestra el  botón de crear una nueva propuesta este botón esta activo solo para los que pueden crear y el botón de ver todas. 

/listar_borradores, /listar_publicados, /listar_archivados -> Devuelve una lista de todas las propuestas pero filtradas por el tipo de estado borrador (id_tipo_estado_propuesta). Usuarios con roles que tengan acceso al modulo propuestas, operaciónes tipo "Ver Propuestas".

/listar_todos -> HTML  que Devuelve una lista de todas las propuestas paginadas y permite realizar busquedas. tabla con todos los cursos listados paginados y con barra de búsqueda. Filtro por tipo y por unidad si el rol lo permite. Aca también se ve el boton de crear nuevo si el rol lo permite. 

/crear: HTML formulario que permite la carga de una nuva propuesta. usuarios con el rol de cargar nuevas propuestas 



/editar/{idpropuesta}: HTML permite editar una propuesta, si es estado borrador se podra editar todo el contenido, si es tipo archivado o publicado no se puede editar contenido basico como el titulo nombre y la unidad a la que pertenece yalgunas mas.

/guardar: url para guardar una propuesta como borrador. Siempre despues del formulario /crear_nuevo guardará como borrador siempre carga la propuesta como borrador en la base de datos solo usuarios con el rol de cargar nuevas propuestas 


/{idpropuesta}: muestra la propuesta y permite editar si esta en borrador o permite publicar si el rol lo permite

/publicar: cambia el estadio del borrador a publicado no se puede modificar mas los atributos claves solo algunos.

/reportes/top_incriptos -> muestra las propuestas en orden de los que tengan ams incriptos se puede mostrar por unidad o de todas las unidades.

/descargar_pdf: descarga cualquier tabla mostrada en pdf
/descargar_csv: descarga cualquier tabla mostrada en csv o xls 

/descargar_alta: descarga un pdf con el alta de una propuesta publicada con la información. muestra un pdf formal diciendo que la propuesta ya ha sido publuicada con la informacion como fecha contenido etc.

Roles
Listas de propuestas en inicio: Todos los roles.
Botón crear nueva propuesta: Ruta y botón activo solo para roles que lo permiten.
