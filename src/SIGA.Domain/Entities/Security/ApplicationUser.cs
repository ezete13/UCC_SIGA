using Microsoft.AspNetCore.Identity;

namespace SIGA.Domain.Entities.Security;

public class ApplicationUser : IdentityUser<int>
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    // Relación con UsuarioUnidad
    public virtual ICollection<UsuarioUnidad> UsuarioUnidades { get; set; } = new List<UsuarioUnidad>();
}
