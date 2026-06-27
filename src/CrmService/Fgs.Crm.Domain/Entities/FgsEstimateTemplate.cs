using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimateTemplate : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long CategoryId { get; set; }

    public string TemplateCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? TemplateDescription { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool ShowToFieldTechnician { get; set; } = true;

    public bool IsActive { get; set; } = true;
}
