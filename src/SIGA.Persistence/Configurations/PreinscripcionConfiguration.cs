using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGA.Domain.Entities.Core;

namespace SIGA.Infrastructure.Data.Configurations;

public class PreinscripcionConfiguration : IEntityTypeConfiguration<Preinscripcion>
{
    public void Configure(EntityTypeBuilder<Preinscripcion> builder)
    {
        ConfigureTable(builder);
        ConfigureProperties(builder);
        ConfigureIndexes(builder);
        ConfigureRelationships(builder);
        ConfigureQueryFilters(builder);
        // No hay seeds para Preinscripcion
    }

    private static void ConfigureTable(EntityTypeBuilder<Preinscripcion> builder)
    {
        builder.ToTable(
            "preinscripciones",
            "siga",
            tb =>
                tb.HasComment("Registro de preinscripciones de interesados a propuestas académicas")
        );

        builder.HasKey(e => e.Id).HasName("pk_preinscripcion");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Preinscripcion> builder)
    {
        // ID - PostgreSQL usa serial/identity
        builder
            .Property(e => e.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn()
            .HasComment("Identificador único de la preinscripción.");

        // UUID - PostgreSQL usa gen_random_uuid()
        builder
            .Property(e => e.Uuid)
            .HasColumnName("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired()
            .HasColumnType("uuid")
            .HasComment("UUID único para identificación universal de la preinscripción.");

        // Foreign Keys
        builder
            .Property(e => e.AlumnoId)
            .HasColumnName("alumno_id")
            .HasColumnType("integer")
            .HasComment("ID del alumno generado al aprobar la preinscripción (FK a alumnos.id).");

        builder
            .Property(e => e.PropuestaVersionId)
            .HasColumnName("propuesta_version_id")
            .IsRequired()
            .HasColumnType("integer")
            .HasComment("ID de la propuesta académica solicitada (FK a propuestas_versiones.id).");

        builder
            .Property(e => e.EstadoPreinscripcionId)
            .HasColumnName("estado_preinscripcion_id")
            .IsRequired()
            .HasColumnType("integer")
            .HasComment("ID del estado de la preinscripción (FK a preinscripcion_estados.id).");

        builder
            .Property(e => e.TipoDocumentoId)
            .HasColumnName("tipo_documento_id")
            .IsRequired()
            .HasColumnType("integer")
            .HasComment("ID del tipo de documento (FK a tipos_documento.id).");

        // Datos personales del preinscripto
        builder
            .Property(e => e.Documento)
            .HasColumnName("documento")
            .HasMaxLength(50)
            .IsRequired()
            .HasColumnType("varchar(50)")
            .HasComment("Número de documento del preinscripto.");

        builder
            .Property(e => e.Apellido)
            .HasColumnName("apellido")
            .HasMaxLength(100)
            .IsRequired()
            .HasColumnType("varchar(100)")
            .HasComment("Apellido(s) del preinscripto.");

        builder
            .Property(e => e.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired()
            .HasColumnType("varchar(100)")
            .HasComment("Nombre(s) del preinscripto.");

        builder
            .Property(e => e.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasComment("Correo electrónico del preinscripto.");

        builder
            .Property(e => e.Telefono)
            .HasColumnName("telefono")
            .HasMaxLength(20)
            .HasColumnType("varchar(20)")
            .HasComment("Número de teléfono de contacto.");

        builder
            .Property(e => e.Observaciones)
            .HasColumnName("observaciones")
            .HasMaxLength(1000)
            .HasColumnType("varchar(1000)")
            .HasComment("Observaciones o motivo de revocación.");

        // Auditoría
        builder
            .Property(e => e.CreadoEn)
            .HasColumnName("creado_en")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasComment("Fecha y hora de creación de la preinscripción.");

        builder
            .Property(e => e.ActualizadoEn)
            .HasColumnName("actualizado_en")
            .HasColumnType("timestamp with time zone")
            .HasComment("Fecha y hora de la última actualización.");
    }

    private static void ConfigureIndexes(EntityTypeBuilder<Preinscripcion> builder)
    {
        // Índices únicos
        builder.HasIndex(e => e.Uuid).IsUnique().HasDatabaseName("uk_preinscripciones_uuid");

        // Índice único compuesto para evitar duplicados de preinscripción activa
        builder
            .HasIndex(e => new
            {
                e.TipoDocumentoId,
                e.Documento,
                e.PropuestaVersionId,
            })
            .IsUnique()
            .HasDatabaseName("uk_preinscripciones_documento_propuesta_version")
            .HasFilter("estado_preinscripcion_id IN (1, 2)"); // En Espera (1) y Aprobada (2)

        // Índices para búsqueda por persona
        builder
            .HasIndex(e => new { e.Apellido, e.Nombre })
            .HasDatabaseName("ix_preinscripciones_apellido_nombre");

        builder.HasIndex(e => e.Apellido).HasDatabaseName("ix_preinscripciones_apellido");

        builder.HasIndex(e => e.Nombre).HasDatabaseName("ix_preinscripciones_nombre");

        builder.HasIndex(e => e.Email).HasDatabaseName("ix_preinscripciones_email");

        builder.HasIndex(e => e.Documento).HasDatabaseName("ix_preinscripciones_documento");

        // Índices para FK
        builder.HasIndex(e => e.PropuestaVersionId).HasDatabaseName("ix_preinscripciones_propuesta_version");

        builder
            .HasIndex(e => e.EstadoPreinscripcionId)
            .HasDatabaseName("ix_preinscripciones_estado");

        builder
            .HasIndex(e => e.TipoDocumentoId)
            .HasDatabaseName("ix_preinscripciones_tipo_documento");

        builder
            .HasIndex(e => e.AlumnoId)
            .HasDatabaseName("ix_preinscripciones_alumno")
            .HasFilter("alumno_id IS NOT NULL");

        // Índices compuestos para filtrado común
        builder
            .HasIndex(e => new { e.PropuestaVersionId, e.EstadoPreinscripcionId })
            .HasDatabaseName("ix_preinscripciones_propuesta_version_estado");

        builder
            .HasIndex(e => new { e.EstadoPreinscripcionId, e.CreadoEn })
            .HasDatabaseName("ix_preinscripciones_estado_fecha");

        // Índice de filtro para preinscripciones pendientes
        builder
            .HasIndex(e => e.EstadoPreinscripcionId)
            .HasDatabaseName("ix_preinscripciones_pendientes")
            .HasFilter("estado_preinscripcion_id = 1"); // En Espera
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Preinscripcion> builder)
    {
        // Relación con PropuestaVersion
        builder
            .HasOne(e => e.PropuestaVersion)
            .WithMany(p => p.Preinscripciones)
            .HasForeignKey(e => e.PropuestaVersionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_preinscripciones_propuesta_version");

        // Relación con PreinscripcionEstado
        builder
            .HasOne(e => e.PreinscripcionEstado)
            .WithMany(pe => pe.Preinscripciones)
            .HasForeignKey(e => e.EstadoPreinscripcionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_preinscripciones_estado");

        // Relación con TipoDocumento
        builder
            .HasOne(e => e.TipoDocumento)
            .WithMany(td => td.Preinscripciones)
            .HasForeignKey(e => e.TipoDocumentoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_preinscripciones_tipo_documento");

        // Relación con Alumno (opcional)
        builder
            .HasOne(e => e.Alumno)
            .WithMany(a => a.Preinscripciones)
            .HasForeignKey(e => e.AlumnoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_preinscripciones_alumno");

        // Relación con Inscripcion (uno a uno)
        builder
            .HasOne(e => e.Inscripcion)
            .WithOne(i => i.Preinscripcion)
            .HasForeignKey<Inscripcion>(i => i.PreinscripcionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_preinscripciones_inscripcion");
    }

    private static void ConfigureQueryFilters(EntityTypeBuilder<Preinscripcion> builder)
    {
        // Nota: No se aplica filtro global por defecto
        // Dependiendo de los requisitos, podría querer filtrar solo las activas/no revocadas
        // builder.HasQueryFilter(e => e.EstadoPreinscripcionId != (int)EstadoPreinscripcionEnum.Revocada);
    }
}
