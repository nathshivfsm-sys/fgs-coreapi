namespace Fgs.User.Domain.Entities;

/// <summary>
/// Stores user-specific grid layout and display preferences for each grid within a tenant and company.
/// </summary>
public class FgsUserGridPreference : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public Guid UserId { get; set; }

    public string GridCode { get; set; } = null!;

    public string GridConfiguration { get; set; } = "{}";

    public FgsUser? User { get; set; }
}
