namespace Fgs.User.Domain.Entities;

/// <summary>
/// Scope rule that defines the records included in a data access profile.
/// </summary>
public class FgsDataAccessScope : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long FgsDataAccessId { get; set; }

    public string ScopeType { get; set; } = null!;

    public string Operator { get; set; } = null!;

    public string? ScopeValue { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsDataAccess? FgsDataAccess { get; set; }
}
