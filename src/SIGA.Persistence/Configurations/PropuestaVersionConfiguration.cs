using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGA.Domain.Entities.Core;

namespace SIGA.Infrastructure.Data.Configurations;

public class PropuestaVersionConfiguration : IEntityTypeConfiguration<PropuestaVersion>
{
    public void Configure(EntityTypeBuilder<PropuestaVersion> builder)
    {
        builder.ToTable(
            "propuestas_versiones",
            "siga",
            tb => tb.HasComment("Registro de cohortes o versiones de una propuesta académica")
        );

        builder.HasKey(e => e.Id).HasName("pk_propuesta_version");

        builder.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();
        
        builder.Property(e => e.Uuid)
            .HasColumnName("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(e => e.PropuestaId)
            .HasColumnName("propuesta_id")
            .IsRequired();

        builder.Property(e => e.PeriodoLectivoId)
            .HasColumnName("periodo_lectivo_id")
            .IsRequired();

        builder.Property(e => e.ModalidadId)
            .HasColumnName("modalidad_id")
            .IsRequired();

        builder.Property(e => e.EstadoPropuestaId)
            .HasColumnName("estado_propuesta_id")
            .IsRequired();

        builder.Property(e => e.FechaInicio)
            .HasColumnName("fecha_inicio")
            .IsRequired()
            .HasColumnType("date");

        builder.Property(e => e.FechaFin)
            .HasColumnName("fecha_fin")
            .IsRequired()
            .HasColumnType("date");

        builder.Property(e => e.MaximoAlumnos)
            .HasColumnName("maximo_alumnos")
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(e => e.CuposDisponibles)
            .HasColumnName("cupos_disponibles")
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(e => e.ImporteBase)
            .HasColumnName("importe_base")
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Cuotas)
            .HasColumnName("cuotas")
            .HasColumnType("integer");

        builder.Property(e => e.ConceptoPago)
            .HasColumnName("concepto_pago")
            .HasMaxLength(255)
            .HasColumnType("varchar(255)");

        builder.Property(e => e.PermiteInscripcionesWeb)
            .HasColumnName("permite_inscripciones_web")
            .HasDefaultValue(false)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(e => e.CreadoEn)
            .HasColumnName("creado_en")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp with time zone");

        builder.Property(e => e.ActualizadoEn)
            .HasColumnName("actualizado_en")
            .HasColumnType("timestamp with time zone");

        // Relaciones
        builder.HasOne(e => e.Propuesta)
            .WithMany(p => p.Versiones)
            .HasForeignKey(e => e.PropuestaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_versiones_propuesta");

        builder.HasOne(e => e.PeriodoLectivo)
            .WithMany()
            .HasForeignKey(e => e.PeriodoLectivoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_versiones_periodo");

        builder.HasOne(e => e.Modalidad)
            .WithMany()
            .HasForeignKey(e => e.ModalidadId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_versiones_modalidad");

        builder.HasOne(e => e.PropuestaEstado)
            .WithMany()
            .HasForeignKey(e => e.EstadoPropuestaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_versiones_estado");

        builder.HasMany(e => e.Preinscripciones)
            .WithOne(p => p.PropuestaVersion)
            .HasForeignKey(p => p.PropuestaVersionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_versiones_preinscripciones");

        builder.HasMany(e => e.Inscripciones)
            .WithOne(i => i.PropuestaVersion)
            .HasForeignKey(i => i.PropuestaVersionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_versiones_inscripciones");

        builder.HasMany(e => e.PropuestaDocentes)
            .WithOne(pd => pd.PropuestaVersion)
            .HasForeignKey(pd => pd.PropuestaVersionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_versiones_docentes");
    }
}
