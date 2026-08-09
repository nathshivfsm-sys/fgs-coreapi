using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Reusable truck stock template used during truck commissioning and synchronization.
/// </summary>
public class FgsTruckStockTemplate : FgsTenantCompanySetupEntityBase<long>
{
    public string TemplateCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<FgsTruckStockTemplateItem> Items { get; set; } = new List<FgsTruckStockTemplateItem>();
}
