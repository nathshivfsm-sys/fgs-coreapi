using Fgs.User.Domain.Enums;

namespace Fgs.User.Domain.Entities;

/// <summary>
/// Platform user scoped to a tenant and company.
/// </summary>
public class FgsUser : FgsEntityBase, ITenantCompanyScoped
{
    public Guid Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string Email { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? EntraObjectId { get; set; }

    /// <summary>
    /// Preferred authentication method (1=Password … 5=PasswordWithMfa). Default PasswordOrEmailOtp.
    /// </summary>
    public AuthenticationMethod AuthenticationMethod { get; set; } = AuthenticationMethod.PasswordOrEmailOtp;

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public ICollection<FgsInvitation> Invitations { get; set; } = new List<FgsInvitation>();

    public ICollection<FgsUserRole> UserRoles { get; set; } = new List<FgsUserRole>();
}
