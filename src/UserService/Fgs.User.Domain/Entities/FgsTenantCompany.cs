using Fgs.User.Domain.Enums;

namespace Fgs.User.Domain.Entities;

/// <summary>
/// Tenant company row. <see cref="CompanyGuid"/> is the stable business key used with <see cref="TenantId"/>
/// for composite foreign keys from other <c>Fgs*</c> tables (aligns with schema reference composite to <c>FgsTenantCompany</c>).
/// </summary>
public class FgsTenantCompany : FgsEntityBase
{
    public long Id { get; set; }

    public Guid CompanyGuid { get; set; }

    public Guid TenantId { get; set; }

    public long CompanyNumber { get; set; }

    public int BusinessTypeId { get; set; }

    public CompanySize? CompanySize { get; set; }

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
