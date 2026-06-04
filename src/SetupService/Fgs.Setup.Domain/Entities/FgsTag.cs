namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped tag.
/// </summary>
public class FgsTag : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string? TagCode { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string? Description { get; set; }

    public string? BackgroundColor { get; set; }

    public string? TextColor { get; set; }

    public long? IconFileId { get; set; }

    public int UsageCount { get; set; }

    public bool IsSystemGenerated { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public long? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }

}
