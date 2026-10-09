using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SIGA.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabaseClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "siga");

            migrationBuilder.CreateTable(
                name: "alumno_estados",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del estado (corresponde al valor del enum).")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único del estado"),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre descriptivo del estado para mostrar en UI."),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Descripción detallada del significado del estado."),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el estado está disponible")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alumno_estado", x => x.id);
                },
                comment: "Registro principal de estados de alumnos");

            migrationBuilder.CreateTable(
                name: "certificado_estados",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del estado (corresponde al valor del enum).")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único del estado"),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre descriptivo del estado para mostrar en UI."),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Descripción detallada del significado del estado."),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el estado está disponible")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_certificado_estado", x => x.id);
                },
                comment: "Registro principal de estados de certificados");

            migrationBuilder.CreateTable(
                name: "docentes",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del docente.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Nombre(s) del docente."),
                    apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Apellido(s) del docente."),
                    dni = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "Documento Nacional de Identidad del docente."),
                    profesion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Profesión o título principal del docente."),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "Número de teléfono de contacto."),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Correo electrónico del docente."),
                    especialidad = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "Especialidad o área de expertise del docente."),
                    biografia = table.Column<string>(type: "text", nullable: true, comment: "Biografía o resumen curricular del docente."),
                    linkedin = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "URL del perfil de LinkedIn u otra red profesional."),
                    estado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el docente está activo en el sistema."),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Fecha y hora de creación del registro."),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de la última actualización del registro.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_docente", x => x.id);
                },
                comment: "Registro de docentes y profesionales que dictan propuestas académicas");

            migrationBuilder.CreateTable(
                name: "estado_inscripcion",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del estado de inscripción."),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único del estado (ACTIVA, FINALIZADA, BAJA)."),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre descriptivo del estado para mostrar en UI."),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Descripción detallada del significado del estado."),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el estado está disponible para uso.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_estado_inscripcion", x => x.id);
                },
                comment: "Catálogo de estados posibles para una inscripción");

            migrationBuilder.CreateTable(
                name: "modalidades",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único de la modalidad")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único de la modalidad (PRE, DIS, SEM, ELE, etc.)"),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre descriptivo de la modalidad para mostrar en UI"),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Descripción detallada de la modalidad y sus características"),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si la modalidad está disponible para uso")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_modalidad", x => x.id);
                },
                comment: "Catálogo de modalidades de cursado disponibles para propuestas");

            migrationBuilder.CreateTable(
                name: "periodos_lectivos",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del período lectivo")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false, comment: "Fecha de inicio del período lectivo"),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false, comment: "Fecha de finalización del período lectivo"),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único del período lectivo (24S1, 25AN, etc.)"),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre descriptivo del período lectivo para mostrar en UI"),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Descripción adicional del período lectivo"),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el período lectivo está disponible para uso")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_periodo_lectivo", x => x.id);
                },
                comment: "Períodos lectivos académicos (semestres, anuales, etc.)");

            migrationBuilder.CreateTable(
                name: "preinscripcion_estados",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del estado (corresponde al valor del enum).")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único del estado"),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre descriptivo del estado para mostrar en UI."),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Descripción detallada del significado del estado."),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el estado está disponible")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_preinscripcion_estado", x => x.id);
                },
                comment: "Registro principal de estados de preinscripciones");

            migrationBuilder.CreateTable(
                name: "propuesta_estados",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del estado (corresponde al valor del enum).")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único del estado"),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre descriptivo del estado para mostrar en UI."),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Descripción detallada del significado del estado."),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el estado está disponible")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_propuesta_estado", x => x.id);
                },
                comment: "Registro principal de estados de Propuestas");

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "siga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipos_documento",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del tipo de documento")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único del tipo de documento (DNI, PAS, CI, etc.)"),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre descriptivo del tipo de documento para mostrar en UI"),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Descripción detallada del tipo de documento"),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el tipo de documento está disponible para uso")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipo_documento", x => x.id);
                },
                comment: "Catálogo de tipos de documento disponibles en el sistema");

            migrationBuilder.CreateTable(
                name: "tipos_propuesta",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del tipo de propuesta")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único del tipo de propuesta (GR, TC, PG, CS, etc.)"),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre descriptivo del tipo de propuesta para mostrar en UI"),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Descripción detallada del tipo de propuesta y su alcance"),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el tipo de propuesta está disponible para uso")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipo_propuesta", x => x.id);
                },
                comment: "Catálogo de tipos de propuestas académicas (carreras, cursos, talleres, etc.)");

            migrationBuilder.CreateTable(
                name: "unidades",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único de la unidad")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_corto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nombre abreviado o de uso común de la unidad"),
                    siglas = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Siglas identificadoras de la unidad"),
                    color_principal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Color principal en formato hexadecimal para identificación visual"),
                    color_secundario = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Color secundario en formato hexadecimal para identificación visual"),
                    direccion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Dirección física de la unidad"),
                    telefono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Teléfono de contacto de la unidad"),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Correo electrónico de contacto de la unidad"),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Código único de la unidad (UCCSJ, FCEE, FCM, etc.)"),
                    nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Nombre completo de la unidad académica"),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Descripción adicional de la unidad"),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si la unidad está activa en el sistema")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unidad", x => x.id);
                },
                comment: "Unidades académicas y administrativas (sedes, facultades, escuelas, institutos)");

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "siga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles_claims",
                schema: "siga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_roles_claims_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "siga",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alumnos",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "ID único autoincremental.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()", comment: "UUID único para identificación universal."),
                    tipo_documento_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del tipo de documento (FK a tipos_documento.id)."),
                    alumno_estado_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del estado actual (FK a alumnos_estado.id)."),
                    num_documento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Número de documento único."),
                    apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Apellido(s) del alumno."),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Nombre(s) del alumno."),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: false, comment: "Fecha de nacimiento del alumno."),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Correo electrónico del alumno."),
                    sexo = table.Column<string>(type: "char(1)", maxLength: 1, nullable: true, comment: "Sexo: M - Masculino, F - Femenino, O - Otro."),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "Número de teléfono."),
                    domicilio = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Dirección de residencia."),
                    codigo_postal = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, comment: "Código postal."),
                    ciudad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "Ciudad de residencia."),
                    provincia = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "Provincia/Estado de residencia."),
                    pais = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "País de residencia."),
                    ciudad_nacimiento = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "Ciudad de nacimiento."),
                    colegio = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "Colegio o institución de procedencia."),
                    profesion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "Profesión u ocupación."),
                    lugar_trabajo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "Lugar de trabajo."),
                    numero_socio = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "Número de socio."),
                    es_socio = table.Column<string>(type: "char(1)", maxLength: 1, nullable: true, comment: "Indicador de socio: S - Sí, N - No."),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Estado activo/inactivo del registro."),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Fecha y hora de creación."),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de última actualización.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alumno", x => x.id);
                    table.ForeignKey(
                        name: "fk_estados_alumno_alumnos",
                        column: x => x.alumno_estado_id,
                        principalSchema: "siga",
                        principalTable: "alumno_estados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tipos_documento_alumnos",
                        column: x => x.tipo_documento_id,
                        principalSchema: "siga",
                        principalTable: "tipos_documento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Registro principal de alumnos aceptados de preinscripciones");

            migrationBuilder.CreateTable(
                name: "autoridades",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único de la autoridad.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    unidad_id = table.Column<int>(type: "integer", nullable: false, comment: "ID de la unidad académica a la que pertenece (FK a unidades.id)."),
                    periodo_lectivo_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del período lectivo en el que ejerce el cargo (FK a periodos_lectivos.id)."),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Nombre(s) de la autoridad."),
                    apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Apellido(s) de la autoridad."),
                    cargo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "Cargo o posición que ocupa (Rector, Decano, Secretario, etc.)."),
                    firma_img = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "Ruta o URL de la imagen de la firma escaneada."),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si la autoridad está activa en el sistema."),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Fecha y hora de creación del registro."),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de la última actualización del registro.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_autoridad", x => x.id);
                    table.ForeignKey(
                        name: "fk_periodos_lectivos_autoridades",
                        column: x => x.periodo_lectivo_id,
                        principalSchema: "siga",
                        principalTable: "periodos_lectivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_unidades_autoridades",
                        column: x => x.unidad_id,
                        principalSchema: "siga",
                        principalTable: "unidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Registro de autoridades académicas que firman certificados");

            migrationBuilder.CreateTable(
                name: "propuestas",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    unidad_id = table.Column<int>(type: "integer", nullable: false),
                    tipo_propuesta_id = table.Column<int>(type: "integer", nullable: false),
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    titulo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    cantidad_horas = table.Column<int>(type: "integer", nullable: false),
                    email_encargado = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    plan_estudio_pdf = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    lugar_realizacion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    estado = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    contacto_info = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    pagos_info = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    web_visible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_propuesta", x => x.id);
                    table.ForeignKey(
                        name: "fk_tipos_propuesta_propuestas",
                        column: x => x.tipo_propuesta_id,
                        principalSchema: "siga",
                        principalTable: "tipos_propuesta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_unidades_propuestas",
                        column: x => x.unidad_id,
                        principalSchema: "siga",
                        principalTable: "unidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Registro de propuestas académicas (catálogo)");

            migrationBuilder.CreateTable(
                name: "usuarios_claims",
                schema: "siga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_usuarios_claims_usuarios_UserId",
                        column: x => x.UserId,
                        principalSchema: "siga",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_logins",
                schema: "siga",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_usuarios_logins_usuarios_UserId",
                        column: x => x.UserId,
                        principalSchema: "siga",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_roles",
                schema: "siga",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_usuarios_roles_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "siga",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuarios_roles_usuarios_UserId",
                        column: x => x.UserId,
                        principalSchema: "siga",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_tokens",
                schema: "siga",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_usuarios_tokens_usuarios_UserId",
                        column: x => x.UserId,
                        principalSchema: "siga",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_unidades",
                schema: "siga",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    UnidadId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_unidades", x => new { x.UsuarioId, x.UnidadId });
                    table.ForeignKey(
                        name: "fk_usuarios_unidades_unidad",
                        column: x => x.UnidadId,
                        principalSchema: "siga",
                        principalTable: "unidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usuarios_unidades_usuario",
                        column: x => x.UsuarioId,
                        principalSchema: "siga",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "propuestas_versiones",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    propuesta_id = table.Column<int>(type: "integer", nullable: false),
                    periodo_lectivo_id = table.Column<int>(type: "integer", nullable: false),
                    modalidad_id = table.Column<int>(type: "integer", nullable: false),
                    estado_propuesta_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    maximo_alumnos = table.Column<int>(type: "integer", nullable: false),
                    cupos_disponibles = table.Column<int>(type: "integer", nullable: false),
                    importe_base = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    cuotas = table.Column<int>(type: "integer", nullable: true),
                    concepto_pago = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    permite_inscripciones_web = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModalidadId1 = table.Column<int>(type: "integer", nullable: true),
                    PeriodoLectivoId1 = table.Column<int>(type: "integer", nullable: true),
                    PropuestaEstadoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_propuesta_version", x => x.id);
                    table.ForeignKey(
                        name: "FK_propuestas_versiones_modalidades_ModalidadId1",
                        column: x => x.ModalidadId1,
                        principalSchema: "siga",
                        principalTable: "modalidades",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_propuestas_versiones_periodos_lectivos_PeriodoLectivoId1",
                        column: x => x.PeriodoLectivoId1,
                        principalSchema: "siga",
                        principalTable: "periodos_lectivos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_propuestas_versiones_propuesta_estados_PropuestaEstadoId",
                        column: x => x.PropuestaEstadoId,
                        principalSchema: "siga",
                        principalTable: "propuesta_estados",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_propuestas_versiones_estado",
                        column: x => x.estado_propuesta_id,
                        principalSchema: "siga",
                        principalTable: "propuesta_estados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_propuestas_versiones_modalidad",
                        column: x => x.modalidad_id,
                        principalSchema: "siga",
                        principalTable: "modalidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_propuestas_versiones_periodo",
                        column: x => x.periodo_lectivo_id,
                        principalSchema: "siga",
                        principalTable: "periodos_lectivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_propuestas_versiones_propuesta",
                        column: x => x.propuesta_id,
                        principalSchema: "siga",
                        principalTable: "propuestas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Registro de cohortes o versiones de una propuesta académica");

            migrationBuilder.CreateTable(
                name: "propuestas_web",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único de la landing page.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    propuesta_id = table.Column<int>(type: "integer", nullable: false, comment: "ID de la propuesta académica asociada (FK a propuestas.id)."),
                    titulo_web = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "Título de la landing page (puede diferir del título académico)."),
                    slug = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "Identificador URL amigable único para la landing page."),
                    banner_img = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "URL de la imagen de banner para la landing page."),
                    acerca_de = table.Column<string>(type: "text", nullable: true, comment: "Descripción general de la propuesta para la web."),
                    perfil_estudiante = table.Column<string>(type: "text", nullable: true, comment: "Descripción del perfil del estudiante ideal."),
                    requisitos = table.Column<string>(type: "text", nullable: true, comment: "Requisitos de inscripción y cursado."),
                    destinatarios = table.Column<string>(type: "text", nullable: true, comment: "Público objetivo al que está dirigida la propuesta."),
                    fundamentacion = table.Column<string>(type: "text", nullable: true, comment: "Fundamentación académica y pedagógica."),
                    etiquetas = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Etiquetas para categorización (pueden ser JSON o CSV)."),
                    permite_inscripciones = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true, comment: "Indica si la landing page permite inscripciones online."),
                    meta_og_title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "Título para Open Graph (compartir en redes)."),
                    meta_og_image = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Imagen para Open Graph (compartir en redes)."),
                    meta_description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Meta descripción para SEO."),
                    meta_keywords = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Meta keywords para SEO."),
                    visitas = table.Column<int>(type: "integer", nullable: true, defaultValue: 0, comment: "Contador de visitas a la landing page."),
                    estado = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true, comment: "Indica si la landing page está publicada/activa."),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Fecha y hora de creación del registro."),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de la última actualización del registro.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_propuesta_web", x => x.id);
                    table.ForeignKey(
                        name: "fk_propuestas_web_propuesta",
                        column: x => x.propuesta_id,
                        principalSchema: "siga",
                        principalTable: "propuestas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Landing pages y contenido web para propuestas académicas");

            migrationBuilder.CreateTable(
                name: "temarios",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único del módulo temático.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    propuesta_id = table.Column<int>(type: "integer", nullable: false, comment: "ID de la propuesta académica (FK a propuestas.id)."),
                    titulo_modulo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "Título del módulo o unidad temática."),
                    descripcion = table.Column<string>(type: "text", nullable: true, comment: "Descripción detallada de los contenidos del módulo."),
                    orden = table.Column<int>(type: "integer", nullable: false, comment: "Número de orden del módulo dentro de la propuesta."),
                    horas = table.Column<int>(type: "integer", nullable: true, comment: "Cantidad de horas dedicadas a este módulo (si aplica).")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_temario", x => x.id);
                    table.ForeignKey(
                        name: "fk_temarios_propuesta",
                        column: x => x.propuesta_id,
                        principalSchema: "siga",
                        principalTable: "propuestas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Módulos y contenidos temáticos de las propuestas académicas");

            migrationBuilder.CreateTable(
                name: "preinscripciones",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único de la preinscripción.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()", comment: "UUID único para identificación universal de la preinscripción."),
                    alumno_id = table.Column<int>(type: "integer", nullable: true, comment: "ID del alumno generado al aprobar la preinscripción (FK a alumnos.id)."),
                    propuesta_version_id = table.Column<int>(type: "integer", nullable: false, comment: "ID de la propuesta académica solicitada (FK a propuestas_versiones.id)."),
                    estado_preinscripcion_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del estado de la preinscripción (FK a preinscripcion_estados.id)."),
                    tipo_documento_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del tipo de documento (FK a tipos_documento.id)."),
                    documento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Número de documento del preinscripto."),
                    apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Apellido(s) del preinscripto."),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Nombre(s) del preinscripto."),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Correo electrónico del preinscripto."),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "Número de teléfono de contacto."),
                    observaciones = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "Observaciones o motivo de revocación."),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Fecha y hora de creación de la preinscripción."),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de la última actualización.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_preinscripcion", x => x.id);
                    table.ForeignKey(
                        name: "fk_estados_preinscripcion_preinscripciones",
                        column: x => x.estado_preinscripcion_id,
                        principalSchema: "siga",
                        principalTable: "preinscripcion_estados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_preinscripciones_alumno",
                        column: x => x.alumno_id,
                        principalSchema: "siga",
                        principalTable: "alumnos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_propuestas_versiones_preinscripciones",
                        column: x => x.propuesta_version_id,
                        principalSchema: "siga",
                        principalTable: "propuestas_versiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tipos_documento_preinscripciones",
                        column: x => x.tipo_documento_id,
                        principalSchema: "siga",
                        principalTable: "tipos_documento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Registro de preinscripciones de interesados a propuestas académicas");

            migrationBuilder.CreateTable(
                name: "propuestas_docentes",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único de la relación propuesta-docente.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    propuesta_version_id = table.Column<int>(type: "integer", nullable: false, comment: "ID de la propuesta académica (FK a propuestas_versiones.id)."),
                    docente_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del docente (FK a docentes.id)."),
                    rol = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Rol del docente en la propuesta (Titular, Adjunto, Asistente, Invitado, etc.)."),
                    orden_web = table.Column<int>(type: "integer", nullable: true, comment: "Orden de visualización del docente en la web (para ordenar por jerarquía).")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_propuesta_docente", x => x.id);
                    table.ForeignKey(
                        name: "fk_propuestas_docentes_docente",
                        column: x => x.docente_id,
                        principalSchema: "siga",
                        principalTable: "docentes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_propuestas_versiones_docentes",
                        column: x => x.propuesta_version_id,
                        principalSchema: "siga",
                        principalTable: "propuestas_versiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Tabla intermedia que relaciona docentes con propuestas académicas");

            migrationBuilder.CreateTable(
                name: "inscripciones",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Identificador único de la inscripción.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()", comment: "UUID único para identificación universal de la inscripción."),
                    alumno_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del alumno inscrito (FK a alumnos.id)."),
                    propuesta_version_id = table.Column<int>(type: "integer", nullable: false, comment: "ID de la propuesta académica (FK a propuestas_versiones.id)."),
                    inscripcion_estado_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del estado de la inscripción (FK a inscripcion_estados.id)."),
                    preinscripcion_id = table.Column<int>(type: "integer", nullable: true, comment: "ID de la preinscripción que originó la inscripción (FK a preinscripciones.id)."),
                    fecha_inscripcion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Fecha y hora de la inscripción."),
                    es_baja = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indica si la inscripción fue dada de baja."),
                    fecha_baja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de la baja (si aplica)."),
                    motivo_baja = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Motivo de la baja de inscripción."),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Fecha y hora de creación del registro."),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de la última actualización del registro.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inscripcion", x => x.id);
                    table.ForeignKey(
                        name: "fk_estados_inscripcion_inscripciones",
                        column: x => x.inscripcion_estado_id,
                        principalSchema: "siga",
                        principalTable: "estado_inscripcion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_inscripciones_alumno",
                        column: x => x.alumno_id,
                        principalSchema: "siga",
                        principalTable: "alumnos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_preinscripciones_inscripcion",
                        column: x => x.preinscripcion_id,
                        principalSchema: "siga",
                        principalTable: "preinscripciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_propuestas_versiones_inscripciones",
                        column: x => x.propuesta_version_id,
                        principalSchema: "siga",
                        principalTable: "propuestas_versiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Registro de inscripciones de alumnos a propuestas académicas");

            migrationBuilder.CreateTable(
                name: "certificados",
                schema: "siga",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "ID único autoincremental del certificado.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()", comment: "UUID único para identificación pública del certificado."),
                    token = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Token único para verificación del certificado."),
                    inscripcion_id = table.Column<int>(type: "integer", nullable: false, comment: "ID de la inscripción relacionada (FK a inscripciones.id)."),
                    alumno_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del alumno certificado (FK a alumnos.id)."),
                    certificado_estado_id = table.Column<int>(type: "integer", nullable: false, comment: "ID del estado del certificado (FK a certificado_estados.id)."),
                    version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1, comment: "Número de versión del certificado (para reemisiones)."),
                    es_version_actual = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si esta es la versión actual del certificado."),
                    hash_seguridad = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, comment: "Hash de seguridad para validar integridad del certificado."),
                    titulo_certificado = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "Título oficial del certificado."),
                    texto_certificado = table.Column<string>(type: "text", nullable: true, comment: "Texto completo del certificado (puede incluir HTML o Markdown)."),
                    horas_certificadas = table.Column<int>(type: "integer", nullable: true, comment: "Número total de horas certificadas."),
                    nota_final = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, comment: "Nota final obtenida (formato según normativa)."),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: true, comment: "Fecha de inicio del curso/certificación."),
                    fecha_finalizacion = table.Column<DateOnly>(type: "date", nullable: true, comment: "Fecha de finalización del curso/certificación."),
                    fecha_emision = table.Column<DateOnly>(type: "date", nullable: false, comment: "Fecha de emisión del certificado."),
                    usuario_id = table.Column<int>(type: "integer", nullable: true, comment: "ID del usuario que emitió/validó el certificado (FK a usuarios.id)."),
                    fecha_validacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de validación del certificado."),
                    ip_validacion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, comment: "Dirección IP desde donde se validó el certificado."),
                    url_verificacion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "URL única para verificación pública del certificado."),
                    ruta_almacenamiento_pdf = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Ruta del archivo PDF generado (si aplica)."),
                    fecha_revocacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de revocación del certificado."),
                    motivo_revocacion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Motivo de la revocación del certificado."),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indica si el registro del certificado está activo."),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Fecha y hora de creación del registro."),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Fecha y hora de última actualización.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_certificado", x => x.id);
                    table.ForeignKey(
                        name: "fk_certificados_alumno",
                        column: x => x.alumno_id,
                        principalSchema: "siga",
                        principalTable: "alumnos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_estados_certificado_certificados",
                        column: x => x.certificado_estado_id,
                        principalSchema: "siga",
                        principalTable: "certificado_estados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_inscripciones_certificados",
                        column: x => x.inscripcion_id,
                        principalSchema: "siga",
                        principalTable: "inscripciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Registro de certificados emitidos para alumnos.");

            migrationBuilder.InsertData(
                schema: "siga",
                table: "alumno_estados",
                columns: new[] { "id", "activo", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, "ACT", "Alumno con al menos una inscripción activa", "Activo" },
                    { 2, true, "INA", "Alumno con ninguna inscripción activa", "Inactivo" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "alumno_estados",
                columns: new[] { "id", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 3, "BLO", "Alumno que no puede obtener ninguna inscripción", "Bloqueado" },
                    { 4, "SUS", "Alumno suspendido temporalmente", "Suspendido" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "certificado_estados",
                columns: new[] { "id", "activo", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, "NOG", "Estado inicial: el alumno aún no cumple requisitos. El certificado no puede generarse, editarse ni eliminarse.", "No Generable" },
                    { 2, true, "PEN", "El alumno cumple requisitos. El certificado puede generarse pero aún no existe emitido. No permite edición.", "Pendiente" },
                    { 3, true, "GEN", "El certificado fue emitido y registrado. Solo puede modificarse mediante versionado y no puede eliminarse.", "Generado" },
                    { 4, true, "APR", "Certificado validado institucionalmente. No puede modificarse ni eliminarse y es plenamente oficial.", "Aprobado" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "certificado_estados",
                columns: new[] { "id", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 5, "INH", "Certificado temporalmente no válido por revisión o inconsistencias. No puede editarse ni eliminarse. El QR refleja no validez.", "Inhabilitado" },
                    { 6, "REV", "Certificado invalidado definitivamente. No puede modificarse ni eliminarse. El QR indica revocación y no validez.", "Revocado" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "estado_inscripcion",
                columns: new[] { "id", "activo", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, "ACT", "Inscripción vigente con cursada regular", "Activa" },
                    { 2, true, "FIN", "Inscripción completada exitosamente", "Finalizada" },
                    { 3, true, "BAJA", "Inscripción cancelada por el alumno o la institución", "Baja" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "modalidades",
                columns: new[] { "id", "activo", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, "PRE", "Clases 100% en sede física con asistencia obligatoria a todas las actividades académicas", "Presencial" },
                    { 2, true, "DIS", "Clases 100% en línea sin requerimiento de presencia física, utilizando plataformas digitales", "A Distancia" },
                    { 3, true, "SEM", "Modalidad con mayor porcentaje virtual complementado con sesiones presenciales obligatorias específicas", "Semipresencial" },
                    { 4, true, "ELE", "El estudiante puede elegir entre asistir presencialmente o seguir virtualmente cada clase según su preferencia", "A Elección" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "periodos_lectivos",
                columns: new[] { "id", "activo", "codigo", "descripcion", "fecha_fin", "fecha_inicio", "nombre" },
                values: new object[,]
                {
                    { 1, true, "24S1", "", new DateOnly(2024, 6, 30), new DateOnly(2024, 3, 1), "Primer Semestre 2024" },
                    { 2, true, "24S2", "", new DateOnly(2024, 11, 30), new DateOnly(2024, 8, 1), "Segundo Semestre 2024" },
                    { 3, true, "25S1", "", new DateOnly(2025, 6, 30), new DateOnly(2025, 3, 1), "Primer Semestre 2025" },
                    { 4, true, "25S2", "", new DateOnly(2025, 11, 30), new DateOnly(2025, 8, 1), "Segundo Semestre 2025" },
                    { 5, true, "24AN", "", new DateOnly(2024, 11, 30), new DateOnly(2024, 3, 1), "Ciclo Anual 2024" },
                    { 6, true, "25AN", "", new DateOnly(2025, 11, 30), new DateOnly(2025, 3, 1), "Ciclo Anual 2025" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "preinscripcion_estados",
                columns: new[] { "id", "activo", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, "PEN", "Preinscripción pendiente de revisión", "Pendiente" },
                    { 2, true, "APR", "Preinscripción aprobada y convertida a alumno", "Aprobada" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "preinscripcion_estados",
                columns: new[] { "id", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 3, "REV", "Preinscripción rechazada o cancelada", "Revocada" },
                    { 4, "EXP", "Preinscripción no procesada dentro del plazo establecido", "Expirada" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "propuesta_estados",
                columns: new[] { "id", "activo", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, "BOR", "Propuesta provisoria no disponible para inscripción", "Borrador" },
                    { 2, true, "PUB", "Propuesta disponible para inscripcion y vista web", "Publicada" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "propuesta_estados",
                columns: new[] { "id", "codigo", "descripcion", "nombre" },
                values: new object[] { 3, "ARCH", "Propuesta no permite vista web ni inscripciones. Vale para certificados", "Archivada" });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "tipos_documento",
                columns: new[] { "id", "activo", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, "DNI", "Documento Nacional de Identidad Argentino", "DNI" },
                    { 2, true, "PAS", "Pasaporte para extranjeros", "Pasaporte" },
                    { 3, true, "CI", "Cédula de identidad extranjera", "Cédula" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "tipos_propuesta",
                columns: new[] { "id", "activo", "codigo", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, "GR", "Carrera de grado, licenciaturas y titulaciones universitarias de nivel superior", "Grado" },
                    { 2, true, "TC", "Formación técnica superior de carácter profesional y especializado", "Técnicatura" },
                    { 3, true, "PG", "Estudios de especialización, maestrías y doctorados posteriores al grado", "Postgrado" },
                    { 4, true, "CS", "Programas de formación específica de duración determinada", "Curso" },
                    { 5, true, "JR", "Certificación por participación en jornadas académicas, congresos o eventos", "Jornada" },
                    { 6, true, "AC", "Programas destinados a la actualización de conocimientos profesionales", "Actualización" },
                    { 7, true, "SM", "Seminarios académicos de formación intensiva y especializada", "Seminario" },
                    { 8, true, "TL", "Talleres prácticos para desarrollo de habilidades específicas", "Taller" },
                    { 9, true, "CP", "Programas de capacitación laboral y desarrollo profesional", "Capacitación" },
                    { 10, true, "HB", "Certificación para habilitación profesional en áreas reguladas", "Habilitación" }
                });

            migrationBuilder.InsertData(
                schema: "siga",
                table: "unidades",
                columns: new[] { "id", "activo", "codigo", "color_principal", "color_secundario", "descripcion", "direccion", "email", "nombre", "nombre_corto", "siglas", "telefono" },
                values: new object[,]
                {
                    { 1, true, "UCCSJ", "#064a31", "#7d1b1c", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "secretariaacademica@uccuyo.edu.ar", "San Juan - Universidad Católica de Cuyo", "UCCuyoSJ", "UCCSJ", "+54 264 4292300" },
                    { 2, true, "UCCSL", "#064a31", "#7d1b1c", "", "Felipe Velázquez 471, D5700 San Luis", "sec.extension@uccuyosl.edu.ar", "San Luis - Universidad Católica de Cuyo", "UCCuyoSL", "UCCSL", "+54 266 4423572" },
                    { 3, true, "UCCMZ", "#064a31", "#7d1b1c", "", "Ruta Provincial 50, M5529 Rodeo del Medio, Mendoza", "enologia@uccuyo.edu.ar", "Mendoza - Universidad Católica de Cuyo", "UCCuyoMZ", "UCCMZ", "+54 261 4951120" },
                    { 4, true, "FCEE", "#1B3B82", "#4A90E2", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "fcee@uccuyo.edu.ar", "Facultad de Ciencias Económicas", "Ciencias Económicas", "FCEE", "+54 264 4292323" },
                    { 5, true, "FDCS", "#691D18", "#795548", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "fdcs@uccuyo.edu.ar", "Facultad de Derecho y Ciencias Sociales", "Derecho y Cs. Sociales", "FDCS", "+54 264 4292335" },
                    { 6, true, "FCM", "#436600", "#4CAF50", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "fcm@uccuyo.edu.ar", "Facultad de Ciencias Médicas", "Ciencias Médicas", "FCM", "+54 264 4292361" },
                    { 7, true, "FFYH", "#BF2C22", "#F44336", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "ffyh@uccuyo.edu.ar", "Facultad de Filosofía y Humanidades", "Filosofía y Humanidades", "FFYH", "+54 264 4292344" },
                    { 8, true, "FCQT", "#00858A", "#00BCD4", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "fcqt@uccuyo.edu.ar", "Facultad de Ciencias Químicas y Tecnológicas", "Cs. Químicas y Tecnológicas", "FCQT", "+54 264 4292357" },
                    { 9, true, "ISFDSM", "#8286D5", "#9C27B0", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "isfdsm@uccuyo.edu.ar", "Instituto Superior Santa María", "Santa María", "ISFDSM", "+54 264 4292300" },
                    { 10, true, "FE", "#E7117E", "#E91E63", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "fe@uccuyo.edu.ar", "Facultad de Educación", "Educación", "FE", "+54 264 4292347" },
                    { 11, true, "ES", "#021233", "#2196F3", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "es@uccuyo.edu.ar", "Escuela de Seguridad", "Seguridad", "ES", "+54 264 4231534" },
                    { 12, true, "ECRP", "#034A31", "#4CAF50", "", "Av. Ignacio de la Roza 1516 Oeste, J5400 Rivadavia, San Juan", "ecrp@uccuyo.edu.ar", "Escuela de Cultura Religiosa y Pastoral", "Cultura Religiosa", "ECRP", "+54 264 4292329" },
                    { 13, true, "FCV", "#5107B4", "#7C4DFF", "", "Felipe Velázquez 471, D5700 San Luis", "fcv@uccuyosl.edu.ar", "Facultad de Ciencias Veterinarias", "Ciencias Veterinarias", "FCV", "+54 266 4423572" },
                    { 14, true, "FDBEYCA", "#CA1F32", "#FF5252", "", "Ruta Provincial 50, M5529 Rodeo del Medio, Mendoza", "enologia@uccuyo.edu.ar", "Facultad de Enología y Alimentación", "Enología y Alimentación", "FDBEYCA", "+54 261 4951120" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_estados_alumno_activo",
                schema: "siga",
                table: "alumno_estados",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_estados_alumno_codigo",
                schema: "siga",
                table: "alumno_estados",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_estados_alumno_nombre",
                schema: "siga",
                table: "alumno_estados",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_alumnos_activo",
                schema: "siga",
                table: "alumnos",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_alumnos_apellido",
                schema: "siga",
                table: "alumnos",
                column: "apellido");

            migrationBuilder.CreateIndex(
                name: "ix_alumnos_apellido_nombre",
                schema: "siga",
                table: "alumnos",
                columns: new[] { "apellido", "nombre" });

            migrationBuilder.CreateIndex(
                name: "ix_alumnos_estado",
                schema: "siga",
                table: "alumnos",
                column: "alumno_estado_id");

            migrationBuilder.CreateIndex(
                name: "ix_alumnos_fecha_nacimiento",
                schema: "siga",
                table: "alumnos",
                column: "fecha_nacimiento");

            migrationBuilder.CreateIndex(
                name: "ix_alumnos_nombre",
                schema: "siga",
                table: "alumnos",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_alumnos_tipo_documento",
                schema: "siga",
                table: "alumnos",
                column: "tipo_documento_id");

            migrationBuilder.CreateIndex(
                name: "ix_alumnos_tipo_documento_numero",
                schema: "siga",
                table: "alumnos",
                columns: new[] { "tipo_documento_id", "num_documento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_alumnos_email",
                schema: "siga",
                table: "alumnos",
                column: "email",
                unique: true,
                filter: "email IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "uk_alumnos_num_documento",
                schema: "siga",
                table: "alumnos",
                column: "num_documento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_alumnos_uuid",
                schema: "siga",
                table: "alumnos",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_autoridades_activo",
                schema: "siga",
                table: "autoridades",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_autoridades_apellido",
                schema: "siga",
                table: "autoridades",
                column: "apellido");

            migrationBuilder.CreateIndex(
                name: "ix_autoridades_apellido_nombre",
                schema: "siga",
                table: "autoridades",
                columns: new[] { "apellido", "nombre" });

            migrationBuilder.CreateIndex(
                name: "ix_autoridades_cargo",
                schema: "siga",
                table: "autoridades",
                column: "cargo");

            migrationBuilder.CreateIndex(
                name: "ix_autoridades_nombre",
                schema: "siga",
                table: "autoridades",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_autoridades_periodo_lectivo",
                schema: "siga",
                table: "autoridades",
                column: "periodo_lectivo_id");

            migrationBuilder.CreateIndex(
                name: "ix_autoridades_unidad",
                schema: "siga",
                table: "autoridades",
                column: "unidad_id");

            migrationBuilder.CreateIndex(
                name: "ix_autoridades_unidad_periodo",
                schema: "siga",
                table: "autoridades",
                columns: new[] { "unidad_id", "periodo_lectivo_id" });

            migrationBuilder.CreateIndex(
                name: "uk_autoridades_unidad_periodo_persona_cargo",
                schema: "siga",
                table: "autoridades",
                columns: new[] { "unidad_id", "periodo_lectivo_id", "apellido", "nombre", "cargo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_estados_certificado_activo",
                schema: "siga",
                table: "certificado_estados",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_estados_certificado_codigo",
                schema: "siga",
                table: "certificado_estados",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_estados_certificado_nombre",
                schema: "siga",
                table: "certificado_estados",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_certificados_activo",
                schema: "siga",
                table: "certificados",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_certificados_alumno",
                schema: "siga",
                table: "certificados",
                column: "alumno_id");

            migrationBuilder.CreateIndex(
                name: "ix_certificados_alumno_version_actual",
                schema: "siga",
                table: "certificados",
                columns: new[] { "alumno_id", "es_version_actual" });

            migrationBuilder.CreateIndex(
                name: "ix_certificados_estado",
                schema: "siga",
                table: "certificados",
                column: "certificado_estado_id");

            migrationBuilder.CreateIndex(
                name: "ix_certificados_fecha_emision",
                schema: "siga",
                table: "certificados",
                column: "fecha_emision");

            migrationBuilder.CreateIndex(
                name: "ix_certificados_fecha_revocacion",
                schema: "siga",
                table: "certificados",
                column: "fecha_revocacion",
                filter: "fecha_revocacion IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_certificados_inscripcion",
                schema: "siga",
                table: "certificados",
                column: "inscripcion_id");

            migrationBuilder.CreateIndex(
                name: "ix_certificados_inscripcion_version_actual",
                schema: "siga",
                table: "certificados",
                columns: new[] { "inscripcion_id", "es_version_actual" });

            migrationBuilder.CreateIndex(
                name: "ix_certificados_rango_fechas",
                schema: "siga",
                table: "certificados",
                columns: new[] { "fecha_inicio", "fecha_finalizacion" });

            migrationBuilder.CreateIndex(
                name: "ix_certificados_titulo",
                schema: "siga",
                table: "certificados",
                column: "titulo_certificado");

            migrationBuilder.CreateIndex(
                name: "ix_certificados_usuario",
                schema: "siga",
                table: "certificados",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_certificados_version_actual",
                schema: "siga",
                table: "certificados",
                column: "es_version_actual");

            migrationBuilder.CreateIndex(
                name: "uk_certificados_alumno_inscripcion_version",
                schema: "siga",
                table: "certificados",
                columns: new[] { "alumno_id", "inscripcion_id", "version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_certificados_hash_seguridad",
                schema: "siga",
                table: "certificados",
                column: "hash_seguridad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_certificados_token",
                schema: "siga",
                table: "certificados",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_certificados_url_verificacion",
                schema: "siga",
                table: "certificados",
                column: "url_verificacion",
                unique: true,
                filter: "url_verificacion IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "uk_certificados_uuid",
                schema: "siga",
                table: "certificados",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_docentes_apellido",
                schema: "siga",
                table: "docentes",
                column: "apellido");

            migrationBuilder.CreateIndex(
                name: "ix_docentes_apellido_nombre",
                schema: "siga",
                table: "docentes",
                columns: new[] { "apellido", "nombre" });

            migrationBuilder.CreateIndex(
                name: "ix_docentes_apellido_profesion",
                schema: "siga",
                table: "docentes",
                columns: new[] { "apellido", "profesion" });

            migrationBuilder.CreateIndex(
                name: "ix_docentes_especialidad",
                schema: "siga",
                table: "docentes",
                column: "especialidad",
                filter: "especialidad IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_docentes_estado",
                schema: "siga",
                table: "docentes",
                column: "estado",
                filter: "estado = true");

            migrationBuilder.CreateIndex(
                name: "ix_docentes_nombre",
                schema: "siga",
                table: "docentes",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_docentes_profesion",
                schema: "siga",
                table: "docentes",
                column: "profesion");

            migrationBuilder.CreateIndex(
                name: "uk_docentes_dni",
                schema: "siga",
                table: "docentes",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_docentes_email",
                schema: "siga",
                table: "docentes",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_estados_inscripcion_activo",
                schema: "siga",
                table: "estado_inscripcion",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_estados_inscripcion_codigo",
                schema: "siga",
                table: "estado_inscripcion",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_estados_inscripcion_nombre",
                schema: "siga",
                table: "estado_inscripcion",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_alumno",
                schema: "siga",
                table: "inscripciones",
                column: "alumno_id");

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_alumno_estado",
                schema: "siga",
                table: "inscripciones",
                columns: new[] { "alumno_id", "inscripcion_estado_id" });

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_es_baja",
                schema: "siga",
                table: "inscripciones",
                column: "es_baja",
                filter: "es_baja = true");

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_estado",
                schema: "siga",
                table: "inscripciones",
                column: "inscripcion_estado_id");

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_fecha_baja",
                schema: "siga",
                table: "inscripciones",
                column: "fecha_baja",
                filter: "fecha_baja IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_fecha_estado",
                schema: "siga",
                table: "inscripciones",
                columns: new[] { "fecha_inscripcion", "inscripcion_estado_id" });

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_fecha_inscripcion",
                schema: "siga",
                table: "inscripciones",
                column: "fecha_inscripcion");

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_preinscripcion",
                schema: "siga",
                table: "inscripciones",
                column: "preinscripcion_id",
                unique: true,
                filter: "preinscripcion_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_propuesta_version",
                schema: "siga",
                table: "inscripciones",
                column: "propuesta_version_id");

            migrationBuilder.CreateIndex(
                name: "ix_inscripciones_propuesta_version_estado",
                schema: "siga",
                table: "inscripciones",
                columns: new[] { "propuesta_version_id", "inscripcion_estado_id" });

            migrationBuilder.CreateIndex(
                name: "uk_inscripciones_alumno_propuesta_version",
                schema: "siga",
                table: "inscripciones",
                columns: new[] { "alumno_id", "propuesta_version_id" },
                unique: true,
                filter: "inscripcion_estado_id = 1");

            migrationBuilder.CreateIndex(
                name: "uk_inscripciones_uuid",
                schema: "siga",
                table: "inscripciones",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_modalidades_activo",
                schema: "siga",
                table: "modalidades",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_modalidades_codigo",
                schema: "siga",
                table: "modalidades",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_modalidades_nombre",
                schema: "siga",
                table: "modalidades",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_periodos_lectivos_activo",
                schema: "siga",
                table: "periodos_lectivos",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_periodos_lectivos_codigo",
                schema: "siga",
                table: "periodos_lectivos",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_periodos_lectivos_fechas",
                schema: "siga",
                table: "periodos_lectivos",
                columns: new[] { "fecha_inicio", "fecha_fin" });

            migrationBuilder.CreateIndex(
                name: "ix_periodos_lectivos_nombre",
                schema: "siga",
                table: "periodos_lectivos",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_estados_preinscripcion_activo",
                schema: "siga",
                table: "preinscripcion_estados",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_estados_preinscripcion_codigo",
                schema: "siga",
                table: "preinscripcion_estados",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_estados_preinscripcion_nombre",
                schema: "siga",
                table: "preinscripcion_estados",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_alumno",
                schema: "siga",
                table: "preinscripciones",
                column: "alumno_id",
                filter: "alumno_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_apellido",
                schema: "siga",
                table: "preinscripciones",
                column: "apellido");

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_apellido_nombre",
                schema: "siga",
                table: "preinscripciones",
                columns: new[] { "apellido", "nombre" });

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_documento",
                schema: "siga",
                table: "preinscripciones",
                column: "documento");

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_email",
                schema: "siga",
                table: "preinscripciones",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_estado_fecha",
                schema: "siga",
                table: "preinscripciones",
                columns: new[] { "estado_preinscripcion_id", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_nombre",
                schema: "siga",
                table: "preinscripciones",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_pendientes",
                schema: "siga",
                table: "preinscripciones",
                column: "estado_preinscripcion_id",
                filter: "estado_preinscripcion_id = 1");

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_propuesta_version",
                schema: "siga",
                table: "preinscripciones",
                column: "propuesta_version_id");

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_propuesta_version_estado",
                schema: "siga",
                table: "preinscripciones",
                columns: new[] { "propuesta_version_id", "estado_preinscripcion_id" });

            migrationBuilder.CreateIndex(
                name: "ix_preinscripciones_tipo_documento",
                schema: "siga",
                table: "preinscripciones",
                column: "tipo_documento_id");

            migrationBuilder.CreateIndex(
                name: "uk_preinscripciones_documento_propuesta_version",
                schema: "siga",
                table: "preinscripciones",
                columns: new[] { "tipo_documento_id", "documento", "propuesta_version_id" },
                unique: true,
                filter: "estado_preinscripcion_id IN (1, 2)");

            migrationBuilder.CreateIndex(
                name: "uk_preinscripciones_uuid",
                schema: "siga",
                table: "preinscripciones",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_estados_propuesta_activo",
                schema: "siga",
                table: "propuesta_estados",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_estados_propuesta_codigo",
                schema: "siga",
                table: "propuesta_estados",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_estados_propuesta_nombre",
                schema: "siga",
                table: "propuesta_estados",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_tipo_propuesta_id",
                schema: "siga",
                table: "propuestas",
                column: "tipo_propuesta_id");

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_unidad_id",
                schema: "siga",
                table: "propuestas",
                column: "unidad_id");

            migrationBuilder.CreateIndex(
                name: "ix_propuestas_docentes_docente",
                schema: "siga",
                table: "propuestas_docentes",
                column: "docente_id");

            migrationBuilder.CreateIndex(
                name: "ix_propuestas_versiones_docentes_orden_web",
                schema: "siga",
                table: "propuestas_docentes",
                columns: new[] { "propuesta_version_id", "orden_web" });

            migrationBuilder.CreateIndex(
                name: "ix_propuestas_versiones_docentes_propuesta",
                schema: "siga",
                table: "propuestas_docentes",
                column: "propuesta_version_id");

            migrationBuilder.CreateIndex(
                name: "uk_propuestas_versiones_docentes_unique",
                schema: "siga",
                table: "propuestas_docentes",
                columns: new[] { "propuesta_version_id", "docente_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_versiones_estado_propuesta_id",
                schema: "siga",
                table: "propuestas_versiones",
                column: "estado_propuesta_id");

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_versiones_modalidad_id",
                schema: "siga",
                table: "propuestas_versiones",
                column: "modalidad_id");

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_versiones_ModalidadId1",
                schema: "siga",
                table: "propuestas_versiones",
                column: "ModalidadId1");

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_versiones_periodo_lectivo_id",
                schema: "siga",
                table: "propuestas_versiones",
                column: "periodo_lectivo_id");

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_versiones_PeriodoLectivoId1",
                schema: "siga",
                table: "propuestas_versiones",
                column: "PeriodoLectivoId1");

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_versiones_propuesta_id",
                schema: "siga",
                table: "propuestas_versiones",
                column: "propuesta_id");

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_versiones_PropuestaEstadoId",
                schema: "siga",
                table: "propuestas_versiones",
                column: "PropuestaEstadoId");

            migrationBuilder.CreateIndex(
                name: "ix_propuestas_web_estado",
                schema: "siga",
                table: "propuestas_web",
                column: "estado",
                filter: "estado = true");

            migrationBuilder.CreateIndex(
                name: "ix_propuestas_web_estado_inscripciones",
                schema: "siga",
                table: "propuestas_web",
                columns: new[] { "estado", "permite_inscripciones" });

            migrationBuilder.CreateIndex(
                name: "ix_propuestas_web_etiquetas",
                schema: "siga",
                table: "propuestas_web",
                column: "etiquetas");

            migrationBuilder.CreateIndex(
                name: "ix_propuestas_web_permite_inscripciones",
                schema: "siga",
                table: "propuestas_web",
                column: "permite_inscripciones",
                filter: "permite_inscripciones = true");

            migrationBuilder.CreateIndex(
                name: "ix_propuestas_web_titulo",
                schema: "siga",
                table: "propuestas_web",
                column: "titulo_web");

            migrationBuilder.CreateIndex(
                name: "ix_propuestas_web_visitas",
                schema: "siga",
                table: "propuestas_web",
                column: "visitas");

            migrationBuilder.CreateIndex(
                name: "uk_propuestas_web_propuesta",
                schema: "siga",
                table: "propuestas_web",
                column: "propuesta_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_propuestas_web_slug",
                schema: "siga",
                table: "propuestas_web",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "siga",
                table: "roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_claims_RoleId",
                schema: "siga",
                table: "roles_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "ix_temarios_propuesta",
                schema: "siga",
                table: "temarios",
                column: "propuesta_id");

            migrationBuilder.CreateIndex(
                name: "ix_temarios_propuesta_orden",
                schema: "siga",
                table: "temarios",
                columns: new[] { "propuesta_id", "orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_temarios_titulo",
                schema: "siga",
                table: "temarios",
                column: "titulo_modulo");

            migrationBuilder.CreateIndex(
                name: "ix_tipos_documento_activo",
                schema: "siga",
                table: "tipos_documento",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_tipos_documento_codigo",
                schema: "siga",
                table: "tipos_documento",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tipos_documento_nombre",
                schema: "siga",
                table: "tipos_documento",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_tipos_propuesta_activo",
                schema: "siga",
                table: "tipos_propuesta",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_tipos_propuesta_codigo",
                schema: "siga",
                table: "tipos_propuesta",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tipos_propuesta_nombre",
                schema: "siga",
                table: "tipos_propuesta",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_unidades_activo",
                schema: "siga",
                table: "unidades",
                column: "activo",
                filter: "activo = true");

            migrationBuilder.CreateIndex(
                name: "ix_unidades_codigo",
                schema: "siga",
                table: "unidades",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_unidades_nombre",
                schema: "siga",
                table: "unidades",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "ix_unidades_nombre_corto",
                schema: "siga",
                table: "unidades",
                column: "nombre_corto");

            migrationBuilder.CreateIndex(
                name: "ix_unidades_siglas",
                schema: "siga",
                table: "unidades",
                column: "siglas",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "siga",
                table: "usuarios",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "siga",
                table: "usuarios",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_claims_UserId",
                schema: "siga",
                table: "usuarios_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_logins_UserId",
                schema: "siga",
                table: "usuarios_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_roles_RoleId",
                schema: "siga",
                table: "usuarios_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_unidades_UnidadId",
                schema: "siga",
                table: "usuarios_unidades",
                column: "UnidadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "autoridades",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "certificados",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "propuestas_docentes",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "propuestas_web",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "roles_claims",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "temarios",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "usuarios_claims",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "usuarios_logins",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "usuarios_roles",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "usuarios_tokens",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "usuarios_unidades",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "certificado_estados",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "inscripciones",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "docentes",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "estado_inscripcion",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "preinscripciones",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "preinscripcion_estados",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "alumnos",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "propuestas_versiones",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "alumno_estados",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "tipos_documento",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "modalidades",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "periodos_lectivos",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "propuesta_estados",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "propuestas",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "tipos_propuesta",
                schema: "siga");

            migrationBuilder.DropTable(
                name: "unidades",
                schema: "siga");
        }
    }
}
