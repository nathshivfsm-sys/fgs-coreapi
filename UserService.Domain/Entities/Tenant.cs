using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public sealed class Tenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public TenantStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public ICollection<TenantCompany> Subsidiaries { get; private set; } = new List<TenantCompany>();
    public ICollection<User> Users { get; private set; } = new List<User>();
    public ICollection<Invite> Invites { get; private set; } = new List<Invite>();

    private Tenant()
    {
    }

    public static Tenant Create(string name)
    {
        var now = DateTimeOffset.UtcNow;
        return new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name,
            Status = TenantStatus.Active,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
