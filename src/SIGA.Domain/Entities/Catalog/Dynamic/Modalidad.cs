using SIGA.Domain.Common;
using SIGA.Domain.Entities.Core;

namespace SIGA.Domain.Entities.Catalog.Dynamic;

public partial class Modalidad : EntityBase
{
    public virtual ICollection<PropuestaVersion> Propuestas { get; set; } = new List<PropuestaVersion>();
}
