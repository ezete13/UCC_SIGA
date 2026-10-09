using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGA.Domain.Entities.Core;

namespace SIGA.Infrastructure.Data.Configurations;

public class PropuestaConfiguration : IEntityTypeConfiguration<Propuesta>
{
    public void Configure(EntityTypeBuilder<Propuesta> builder)
    {
        builder.ToTable(
            "propuestas",
            "siga",
            tb => tb.HasComment("Registro de propuestas académicas (catálogo)")
        );

        builder.HasKey(e => e.Id).HasName("pk_propuesta");

        builder.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();
        
        builder.Property(e => e.guid).HasColumnName("uuid").HasDefaultValueSql("gen_random_uuid()").HasColumnType("uuid");

        builder.Property(e => e.UnidadId).HasColumnName("unidad_id").IsRequired();
        builder.Property(e => e.TipoPropuestaId).HasColumnName("tipo_propuesta_id").IsRequired();
        builder.Property(e => e.UsuarioId).HasColumnName("usuario_id").IsRequired();

        builder.Property(e => e.Titulo).HasColumnName("titulo").HasMaxLength(500).IsRequired().HasColumnType("varchar(500)");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasColumnType("text");
        
        builder.Property(e => e.CantidadHoras).HasColumnName("cantidad_horas").IsRequired();
        builder.Property(e => e.EmailEncargado).HasColumnName("email_encargado").HasMaxLength(255).HasColumnType("varchar(255)");
        builder.Property(e => e.LugarRealizacion).HasColumnName("lugar_realizacion").HasMaxLength(500).HasColumnType("varchar(500)");
        builder.Property(e => e.ContactoInfo).HasColumnName("contacto_info").HasMaxLength(1000).HasColumnType("varchar(1000)");
        builder.Property(e => e.PagosInfo).HasColumnName("pagos_info").HasMaxLength(1000).HasColumnType("varchar(1000)");
        builder.Property(e => e.PlanEstudioPdf).HasColumnName("plan_estudio_pdf").HasMaxLength(500).HasColumnType("varchar(500)");
        
        builder.Property(e => e.Estado).HasColumnName("estado").HasDefaultValue(true).HasColumnType("boolean");
        builder.Property(e => e.WebVisible).HasColumnName("web_visible").HasDefaultValue(false).IsRequired().HasColumnType("boolean");

        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp with time zone");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamp with time zone");

        // Relationships
        builder.HasOne(e => e.Unidad)
            .WithMany(u => u.Propuestas)
            .HasForeignKey(e => e.UnidadId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_unidad");

        builder.HasOne(e => e.TipoPropuesta)
            .WithMany(tp => tp.Propuestas)
            .HasForeignKey(e => e.TipoPropuestaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_tipo");

        builder.HasOne(e => e.PropuestaWeb)
            .WithOne(pw => pw.Propuesta)
            .HasForeignKey<PropuestaWeb>(pw => pw.PropuestaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_web");

        builder.HasMany(e => e.PropuestaContenidos)
            .WithOne(t => t.Propuesta)
            .HasForeignKey(t => t.PropuestaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_contenidos");
            
        builder.HasMany(e => e.Versiones)
            .WithOne(v => v.Propuesta)
            .HasForeignKey(v => v.PropuestaId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_propuestas_versiones");
    }
}
