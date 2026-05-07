using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Email { get; private set; } = null!;
    public string? DisplayName { get; private set; }
    public UserStatus Status { get; private set; }
    public short CompanyId { get; private set; }
    public UserRole Role { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public Tenant Tenant { get; private set; } = null!;
    public TenantCompany TenantCompany { get; private set; } = null!;
    public ICollection<Invite> Invites { get; private set; } = new List<Invite>();
    public ICollection<AuthIdentity> AuthIdentities { get; private set; } = new List<AuthIdentity>();

    private User()
    {
    }

    public static User CreateAdmin(
        Guid tenantId,
        short companyId,
        string email,
        string? displayName)
    {
        var now = DateTimeOffset.UtcNow;
        return new User
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Email = email,
            DisplayName = displayName,
            Status = UserStatus.Pending,
            CompanyId = companyId,
            Role = UserRole.Admin,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
