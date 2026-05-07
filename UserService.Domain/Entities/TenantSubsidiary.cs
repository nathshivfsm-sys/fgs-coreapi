namespace UserService.Domain.Entities;

public sealed class TenantCompany
{
    public Guid TenantId { get; private set; }
    public short CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public Tenant Tenant { get; private set; } = null!;
    public ICollection<User> Users { get; private set; } = new List<User>();
    public ICollection<Invite> Invites { get; private set; } = new List<Invite>();

    private TenantCompany()
    {
    }

    public static TenantCompany Create(Guid tenantId, short companyId, string name)
    {
        var now = DateTimeOffset.UtcNow;
        return new TenantCompany
        {
            TenantId = tenantId,
            CompanyId = companyId,
            Name = name,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
