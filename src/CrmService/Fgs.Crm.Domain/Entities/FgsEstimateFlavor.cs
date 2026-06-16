using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimateFlavor : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string FlavorCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string BackgroundColor { get; set; } = null!;

    public string TextColor { get; set; } = null!;

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;
}
