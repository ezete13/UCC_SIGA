namespace SIGA.Application.Interfaces.Security;

public interface IProveedorIdentidad
{
    int? ObtenerUsuarioId();
    string? ObtenerEmail();
    string? ObtenerNombre();
    bool EstaAutenticado();
    IEnumerable<string> ObtenerRoles();
    IEnumerable<int> ObtenerUnidadesPermitidas();
}
