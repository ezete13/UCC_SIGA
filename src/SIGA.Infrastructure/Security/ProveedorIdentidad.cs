using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SIGA.Application.Interfaces.Security;

namespace SIGA.Infrastructure.Security;

public class ProveedorIdentidad : IProveedorIdentidad
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProveedorIdentidad(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? ObtenerUsuarioId()
    {
        var idClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (idClaim != null && int.TryParse(idClaim.Value, out int id))
        {
            return id;
        }
        return null;
    }

    public string? ObtenerEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    public string? ObtenerNombre()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public bool EstaAutenticado()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    public IEnumerable<string> ObtenerRoles()
    {
        return _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();
    }

    public IEnumerable<int> ObtenerUnidadesPermitidas()
    {
        return _httpContextAccessor.HttpContext?.User?.FindAll("UnidadId")
            .Select(c => int.TryParse(c.Value, out int id) ? id : (int?)null)
            .Where(id => id.HasValue)
            .Select(id => id!.Value) ?? Enumerable.Empty<int>();
    }
}
