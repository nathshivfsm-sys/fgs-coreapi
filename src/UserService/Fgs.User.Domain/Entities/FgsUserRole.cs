namespace Fgs.User.Domain.Entities;

/// <summary>
/// Assigns one or more security roles to users within a company.
/// </summary>
public class FgsUserRole : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public Guid UserId { get; set; }

    public long FgsRoleId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsUser? User { get; set; }

    public FgsRole? FgsRole { get; set; }
}
