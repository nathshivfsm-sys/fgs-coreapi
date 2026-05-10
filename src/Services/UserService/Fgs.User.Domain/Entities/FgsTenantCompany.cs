namespace Fgs.User.Domain.Entities;

public class FgsTenantCompany : FgsEntityBase
{
    public long Id { get; set; }

    public Guid CompanyGuid { get; set; }

    public Guid TenantId { get; set; }

    public long CompanyNumber { get; set; }

    public int BusinessTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Website { get; set; }

    public string? TaxId { get; set; }

    public Guid? PhysicalLocationId { get; set; }

    public Guid? BillingLocationId { get; set; }

    public string? FullLogoUrl { get; set; }

    public string? CompactLogoUrl { get; set; }

    public string? IconLogoUrl { get; set; }

    public string? FaviconUrl { get; set; }

    public bool IsActive { get; set; } = true;
}
