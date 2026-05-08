namespace UserService.Domain.Entities;

public sealed class Tenant : AuditableEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? LegalName { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Website { get; private set; }
    public Guid? PrimaryLocationId { get; private set; }
    public int? SubscriptionPlanId { get; private set; }
    public string? TimeZone { get; private set; }
    public string? DefaultCurrency { get; private set; }
    public int? DefaultLanguageId { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<Company> Companies { get; private set; } = new List<Company>();
    public ICollection<User> Users { get; private set; } = new List<User>();
    public ICollection<Invite> Invites { get; private set; } = new List<Invite>();

    private Tenant()
    {
    }

    public static Tenant Create(string name)
    {
        var now = DateTime.UtcNow;
        return new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name,
            IsActive = true,
            CreatedOn = now,
            UpdatedOn = now
        };
    }
}
