using SIGA.Domain.Common;
using SIGA.Domain.Entities.Core;

namespace SIGA.Domain.Entities.Catalog.Dynamic;

public partial class PeriodoLectivo : EntityBase
{
    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public virtual ICollection<Autoridad> Autoridades { get; set; } = new List<Autoridad>();

    public virtual ICollection<PropuestaVersion> Propuestas { get; set; } = new List<PropuestaVersion>();
}
