using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGA.Domain.Entities.Core;

namespace SIGA.Infrastructure.Data.Configurations;

public class PropuestaDocenteConfiguration : IEntityTypeConfiguration<PropuestaDocente>
{
    public void Configure(EntityTypeBuilder<PropuestaDocente> builder)
    {
        ConfigureTable(builder);
        ConfigureProperties(builder);
        ConfigureIndexes(builder);
        ConfigureRelationships(builder);
        // No hay seeds para PropuestaDocente (son datos dinámicos)
    }

    private static void ConfigureTable(EntityTypeBuilder<PropuestaDocente> builder)
    {
        builder.ToTable(
            "propuestas_docentes",
            "siga",
            tb => tb.HasComment("Tabla intermedia que relaciona docentes con propuestas académicas")
        );

        builder.HasKey(e => e.Id).HasName("pk_propuesta_docente");
    }

    private static void ConfigureProperties(EntityTypeBuilder<PropuestaDocente> builder)
    {
        // ID - PostgreSQL usa serial/identity
        builder
            .Property(e => e.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn()
            .HasComment("Identificador único de la relación propuesta-docente.");

        // Foreign Keys
        builder
            .Property(e => e.PropuestaVersionId)
            .HasColumnName("propuesta_version_id")
            .IsRequired()
            .HasColumnType("integer")
            .HasComment("ID de la propuesta académica (FK a propuestas_versiones.id).");

        builder
            .Property(e => e.DocenteId)
            .HasColumnName("docente_id")
            .IsRequired()
            .HasColumnType("integer")
            .HasComment("ID del docente (FK a docentes.id).");

        // Información del rol
        builder
            .Property(e => e.Rol)
            .HasColumnName("rol")
            .HasMaxLength(100)
            .IsRequired()
            .HasColumnType("varchar(100)")
            .HasComment(
                "Rol del docente en la propuesta (Titular, Adjunto, Asistente, Invitado, etc.)."
            );

        builder
            .Property(e => e.OrdenWeb)
            .HasColumnName("orden_web")
            .HasColumnType("integer")
            .HasComment(
                "Orden de visualización del docente en la web (para ordenar por jerarquía)."
            );
    }

    private static void ConfigureIndexes(EntityTypeBuilder<PropuestaDocente> builder)
    {
        // Índice único compuesto para evitar duplicados de docente en misma propuesta
        builder
            .HasIndex(e => new { e.PropuestaVersionId, e.DocenteId })
            .IsUnique()
            .HasDatabaseName("uk_propuestas_versiones_docentes_unique");

        // Índices individuales para FK (mejoran performance de joins)
        builder.HasIndex(e => e.PropuestaVersionId).HasDatabaseName("ix_propuestas_versiones_docentes_propuesta");

        builder.HasIndex(e => e.DocenteId).HasDatabaseName("ix_propuestas_docentes_docente");

        // Índice para ordenamiento web
        builder
            .HasIndex(e => new { e.PropuestaVersionId, e.OrdenWeb })
            .HasDatabaseName("ix_propuestas_versiones_docentes_orden_web");
    }

    private static void ConfigureRelationships(EntityTypeBuilder<PropuestaDocente> builder)
    {
        // Relación con PropuestaVersion
        builder
            .HasOne(e => e.PropuestaVersion)
            .WithMany(p => p.PropuestaDocentes)
            .HasForeignKey(e => e.PropuestaVersionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_versiones_docentes_propuesta");

        // Relación con Docente
        builder
            .HasOne(e => e.Docente)
            .WithMany(d => d.PropuestaDocentes)
            .HasForeignKey(e => e.DocenteId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_docentes_docente");
    }
}
