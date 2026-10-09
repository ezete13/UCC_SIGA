namespace SIGA.Domain.Common;

public partial class EntityBase
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; } = null!;

    public bool Activo { get; set; } = true;
}
