namespace Fgs.User.Domain.Entities;

/// <summary>
/// Reusable data access profile that defines the scope of data a role can access.
/// </summary>
public class FgsDataAccess : FgsTenantCompanySetupEntityBase<long>
{
    public string DataAccessCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsBuiltIn { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public ICollection<FgsDataAccessScope> Scopes { get; set; } = [];
}
