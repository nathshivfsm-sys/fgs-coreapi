namespace Fgs.User.Domain.Entities;

/// <summary>
/// Tenant company row. <see cref="CompanyGuid"/> is the stable business key used with <see cref="TenantId"/>
/// for external references. Other <c>Fgs*</c> tables use <c>CompanyId</c> aligned with <see cref="CompanyNumber"/>.
/// </summary>
public class FgsTenantCompany : FgsEntityBase
{
    public long Id { get; set; }

    public Guid CompanyGuid { get; set; }

    public long TenantId { get; set; }

    public long CompanyNumber { get; set; }

    public string? CompanySize { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Website { get; set; }

    public string? TaxId { get; set; }

    public string? TimeZone { get; set; }

    public Guid? PhysicalLocationId { get; set; }

    public Guid? BillingLocationId { get; set; }

    public long? FullLogoUrl { get; set; }

    public long? CompactLogoUrl { get; set; }

    public bool IsActive { get; set; } = true;
}
