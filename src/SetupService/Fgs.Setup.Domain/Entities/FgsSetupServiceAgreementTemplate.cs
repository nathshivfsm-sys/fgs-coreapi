namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Service agreement template defining billing, maintenance frequency, and default terms for a tenant-company.
/// </summary>
public class FgsSetupServiceAgreementTemplate : FgsTenantCompanySetupEntityBase<long>
{
    public string TemplateCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short BillingFrequencyMonths { get; set; }

    public short MaintenanceFrequencyMonths { get; set; }

    public decimal RepairDiscountPercent { get; set; }

    public bool IsAutoRenew { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;
}
