namespace Fgs.User.Domain.Entities;

/// <summary>
/// Assigns data access profiles to security roles within a company.
/// </summary>
public class FgsRoleDataAccess : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long FgsRoleId { get; set; }

    public long FgsDataAccessId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsRole? FgsRole { get; set; }

    public FgsDataAccess? FgsDataAccess { get; set; }
}
