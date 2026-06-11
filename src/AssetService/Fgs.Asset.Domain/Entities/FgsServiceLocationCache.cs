using Fgs.Kernel.Entities;

namespace Fgs.Asset.Domain.Entities;

public class FgsServiceLocationCache : ITenantCompanyScoped
{
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long ServiceLocationId { get; set; }
    public long CustomerId { get; set; }
    public int LocationSequence { get; set; }
    public string LocationNumber { get; set; } = null!;
    public DateTimeOffset? UpdatedOn { get; set; }
}
