using Fgs.User.Domain.Enums;

namespace Fgs.User.Domain.Entities;

/// <summary>
/// Platform user scoped to a tenant and company.
/// </summary>
public class FgsUser : FgsEntityBase
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid CompanyId { get; set; }

    public string Email { get; set; } = null!;

    public string NormalizedEmail { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? EntraObjectId { get; set; }

    public UserRoleType Role { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public FgsTenant? Tenant { get; set; }

    public FgsTenantCompany? Company { get; set; }

    public ICollection<FgsInvitation> Invitations { get; set; } = new List<FgsInvitation>();
}
