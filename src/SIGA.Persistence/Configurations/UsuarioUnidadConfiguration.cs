using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGA.Domain.Entities.Security;

namespace SIGA.Persistence.Configurations;

public class UsuarioUnidadConfiguration : IEntityTypeConfiguration<UsuarioUnidad>
{
    public void Configure(EntityTypeBuilder<UsuarioUnidad> builder)
    {
        builder.ToTable("usuarios_unidades", "siga");

        builder.HasKey(e => new { e.UsuarioId, e.UnidadId });

        builder.HasOne(e => e.Usuario)
            .WithMany(u => u.UsuarioUnidades)
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_usuarios_unidades_usuario");

        builder.HasOne(e => e.Unidad)
            .WithMany()
            .HasForeignKey(e => e.UnidadId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_usuarios_unidades_unidad");
    }
}
