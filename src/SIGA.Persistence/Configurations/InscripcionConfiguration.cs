using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGA.Domain.Entities.Core;

namespace SIGA.Infrastructure.Data.Configurations;

public class InscripcionConfiguration : IEntityTypeConfiguration<Inscripcion>
{
    public void Configure(EntityTypeBuilder<Inscripcion> builder)
    {
        ConfigureTable(builder);
        ConfigureProperties(builder);
        ConfigureIndexes(builder);
        ConfigureRelationships(builder);
        ConfigureQueryFilters(builder);
        // No hay seeds para Inscripcion
    }

    private static void ConfigureTable(EntityTypeBuilder<Inscripcion> builder)
    {
        builder.ToTable(
            "inscripciones",
            "siga",
            tb => tb.HasComment("Registro de inscripciones de alumnos a propuestas académicas")
        );

        builder.HasKey(e => e.Id).HasName("pk_inscripcion");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Inscripcion> builder)
    {
        // ID - PostgreSQL usa serial/identity
        builder
            .Property(e => e.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn()
            .HasComment("Identificador único de la inscripción.");

        // UUID - PostgreSQL usa gen_random_uuid()
        builder
            .Property(e => e.Uuid)
            .HasColumnName("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired()
            .HasColumnType("uuid")
            .HasComment("UUID único para identificación universal de la inscripción.");

        // Foreign Keys requeridas
        builder
            .Property(e => e.AlumnoId)
            .HasColumnName("alumno_id")
            .IsRequired()
            .HasColumnType("integer")
            .HasComment("ID del alumno inscrito (FK a alumnos.id).");

        builder
            .Property(e => e.PropuestaVersionId)
            .HasColumnName("propuesta_version_id")
            .IsRequired()
            .HasColumnType("integer")
            .HasComment("ID de la propuesta académica (FK a propuestas_versiones.id).");

        builder
            .Property(e => e.InscripcionEstadoId)
            .HasColumnName("inscripcion_estado_id")
            .IsRequired()
            .HasColumnType("integer")
            .HasComment("ID del estado de la inscripción (FK a inscripcion_estados.id).");

        // Foreign Key opcional
        builder
            .Property(e => e.PreinscripcionId)
            .HasColumnName("preinscripcion_id")
            .HasColumnType("integer")
            .HasComment(
                "ID de la preinscripción que originó la inscripción (FK a preinscripciones.id)."
            );

        // Fechas
        builder
            .Property(e => e.FechaInscripcion)
            .HasColumnName("fecha_inscripcion")
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasComment("Fecha y hora de la inscripción.");

        // Información de baja
        builder
            .Property(e => e.EsBaja)
            .HasColumnName("es_baja")
            .HasDefaultValue(false)
            .IsRequired()
            .HasColumnType("boolean")
            .HasComment("Indica si la inscripción fue dada de baja.");

        builder
            .Property(e => e.FechaBaja)
            .HasColumnName("fecha_baja")
            .HasColumnType("timestamp with time zone")
            .HasComment("Fecha y hora de la baja (si aplica).");

        builder
            .Property(e => e.MotivoBaja)
            .HasColumnName("motivo_baja")
            .HasMaxLength(500)
            .HasColumnType("varchar(500)")
            .HasComment("Motivo de la baja de inscripción.");

        // Auditoría
        builder
            .Property(e => e.CreadoEn)
            .HasColumnName("creado_en")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasComment("Fecha y hora de creación del registro.");

        builder
            .Property(e => e.ActualizadoEn)
            .HasColumnName("actualizado_en")
            .HasColumnType("timestamp with time zone")
            .HasComment("Fecha y hora de la última actualización del registro.");
    }

    private static void ConfigureIndexes(EntityTypeBuilder<Inscripcion> builder)
    {
        // Índices únicos
        builder.HasIndex(e => e.Uuid).IsUnique().HasDatabaseName("uk_inscripciones_uuid");

        // Índice único compuesto para evitar duplicados de alumno en misma propuesta (solo activas)
        builder
            .HasIndex(e => new { e.AlumnoId, e.PropuestaVersionId })
            .IsUnique()
            .HasDatabaseName("uk_inscripciones_alumno_propuesta_version")
            .HasFilter("inscripcion_estado_id = 1"); // Asumiendo que 1 = Activa

        // Índices para búsquedas frecuentes
        builder.HasIndex(e => e.AlumnoId).HasDatabaseName("ix_inscripciones_alumno");

        builder.HasIndex(e => e.PropuestaVersionId).HasDatabaseName("ix_inscripciones_propuesta_version");

        builder.HasIndex(e => e.InscripcionEstadoId).HasDatabaseName("ix_inscripciones_estado");

        builder
            .HasIndex(e => e.PreinscripcionId)
            .HasDatabaseName("ix_inscripciones_preinscripcion")
            .HasFilter("preinscripcion_id IS NOT NULL");

        // Índices compuestos para consultas comunes
        builder
            .HasIndex(e => new { e.AlumnoId, e.InscripcionEstadoId })
            .HasDatabaseName("ix_inscripciones_alumno_estado");

        builder
            .HasIndex(e => new { e.PropuestaVersionId, e.InscripcionEstadoId })
            .HasDatabaseName("ix_inscripciones_propuesta_version_estado");

        builder
            .HasIndex(e => new { e.FechaInscripcion, e.InscripcionEstadoId })
            .HasDatabaseName("ix_inscripciones_fecha_estado");

        // Índices de filtro
        builder
            .HasIndex(e => e.EsBaja)
            .HasDatabaseName("ix_inscripciones_es_baja")
            .HasFilter("es_baja = true");

        builder
            .HasIndex(e => e.FechaBaja)
            .HasDatabaseName("ix_inscripciones_fecha_baja")
            .HasFilter("fecha_baja IS NOT NULL");

        // Índice para búsqueda por rango de fechas
        builder
            .HasIndex(e => e.FechaInscripcion)
            .HasDatabaseName("ix_inscripciones_fecha_inscripcion");
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Inscripcion> builder)
    {
        // Relación con Alumno
        builder
            .HasOne(e => e.Alumno)
            .WithMany(a => a.Inscripciones)
            .HasForeignKey(e => e.AlumnoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscripciones_alumno");

        // Relación con PropuestaVersion
        builder
            .HasOne(e => e.PropuestaVersion)
            .WithMany(p => p.Inscripciones)
            .HasForeignKey(e => e.PropuestaVersionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscripciones_propuesta_version");

        // Relación con InscripcionEstado
        builder
            .HasOne(e => e.InscripcionEstado)
            .WithMany(ie => ie.Inscripciones)
            .HasForeignKey(e => e.InscripcionEstadoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscripciones_estado");

        // Relación con Certificados
        builder
            .HasMany(e => e.Certificados)
            .WithOne(c => c.Inscripcion)
            .HasForeignKey(c => c.InscripcionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_inscripciones_certificados");
    }

    private static void ConfigureQueryFilters(EntityTypeBuilder<Inscripcion> builder)
    {
        // Filtro global: excluir inscripciones marcadas como baja lógica
        // Nota: Comentado porque puede no ser deseable en todos los casos
        // builder.HasQueryFilter(e => !e.EsBaja);
    }
}
