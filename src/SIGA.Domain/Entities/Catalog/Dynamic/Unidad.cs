using SIGA.Domain.Common;
using SIGA.Domain.Entities.Core;
using SIGA.Domain.Exceptions;

namespace SIGA.Domain.Entities.Catalog.Dynamic;

public partial class Unidad : EntityBase
{
    public string NombreCorto { get; set; } = null!;

    public string Siglas { get; set; } = null!;

    public string? ColorPrincipal { get; set; }

    public string? ColorSecundario { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Autoridad> Autoridades { get; set; } = new List<Autoridad>();

    public virtual ICollection<Propuesta> Propuestas { get; set; } = new List<Propuesta>();

    public static Unidad Crear(
        string codigo,
        string nombre,
        string? descripcion,
        string nombreCorto,
        string siglas,
        string? colorPrincipal,
        string? colorSecundario,
        string? direccion,
        string? telefono,
        string? email
    )
    {
        // Validar que el código no esté vacío o solo contenga espacios
        if (string.IsNullOrWhiteSpace(codigo))
        {
            throw new DomainException("El código de la unidad es obligatorio.");
        }

        // Validar que el código no contenga espacios
        if (string.IsNullOrEmpty(nombre))
        {
            throw new DomainException("El nombre de la unidad es obligatorio.");
        }

        // Validar que el nombre corto no esté vacio
        if (string.IsNullOrEmpty(nombreCorto))
        {
            throw new DomainException("El nombre corto de la unidad es obligatoria.");
        }

        // Validar que las siglas no estén vacías
        if (string.IsNullOrEmpty(siglas))
        {
            throw new DomainException("Las siglas de la unidad son obligatorias.");
        }

        // Validar formato de email
        if (!string.IsNullOrWhiteSpace(email) && !email.Contains("@"))
        {
            throw new DomainException("Email inválido");
        }

        return new Unidad
        {
            Codigo = codigo.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            Descripcion = descripcion,
            NombreCorto = nombreCorto.Trim(),
            Siglas = siglas.Trim().ToUpperInvariant(),
            ColorPrincipal = colorPrincipal,
            ColorSecundario = colorSecundario,
            Direccion = direccion,
            Telefono = telefono,
            Email = email,
        };
    }
}
