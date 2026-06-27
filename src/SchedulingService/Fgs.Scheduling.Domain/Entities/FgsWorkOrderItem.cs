using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsWorkOrderItem : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long WorkOrderId { get; set; }
    public long? InventoryItemId { get; set; }
    public string? ItemName { get; set; }
    public string? Description { get; set; }
    public decimal Quantity { get; set; } = 1.0m;
    public int DisplayOrder { get; set; } = 1;
}
