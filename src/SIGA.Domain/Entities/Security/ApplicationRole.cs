using Microsoft.AspNetCore.Identity;

namespace SIGA.Domain.Entities.Security;

public class ApplicationRole : IdentityRole<int>
{
    public string Descripcion { get; set; } = string.Empty;
}
