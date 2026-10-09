using SIGA.Domain.Entities.Catalog.Dynamic;

namespace SIGA.Domain.Entities.Security;

public class UsuarioUnidad
{
    public int UsuarioId { get; set; }
    public virtual ApplicationUser Usuario { get; set; } = null!;

    public int UnidadId { get; set; }
    public virtual Unidad Unidad { get; set; } = null!;
}
