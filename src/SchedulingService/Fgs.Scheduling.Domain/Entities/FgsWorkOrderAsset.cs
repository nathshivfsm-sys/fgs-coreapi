using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsWorkOrderAsset : ITenantCompanyScoped
{
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long WorkOrderId { get; set; }
    public long AssetId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
}
