using SIGA.Domain.Common;
using SIGA.Domain.Entities.Core;

namespace SIGA.Domain.Entities.Catalog.Static;

public partial class PropuestaEstado : EntityBase
{
    public virtual ICollection<PropuestaVersion> Propuestas { get; set; } = new List<PropuestaVersion>();
}
