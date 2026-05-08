namespace UserService.Domain.Entities;

public sealed class Company : AuditableEntity
{
    public long Id { get; private set; }
    public Guid CompanyGuid { get; private set; }
    public Guid TenantId { get; private set; }
    public long CompanyNumber { get; private set; }
    public int BusinessTypeId { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? LegalName { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Website { get; private set; }
    public string? TaxId { get; private set; }
    public Guid? PrimaryLocationId { get; private set; }
    public string? FullLogoUrl { get; private set; }
    public string? CompactLogoUrl { get; private set; }
    public string? IconLogoUrl { get; private set; }
    public string? FaviconUrl { get; private set; }
    public bool IsActive { get; private set; }

    public Tenant Tenant { get; private set; } = null!;
    public FSGSetupBusinessType BusinessType { get; private set; } = null!;
    public ICollection<User> Users { get; private set; } = new List<User>();
    public ICollection<Invite> Invites { get; private set; } = new List<Invite>();

    private Company()
    {
    }

    public static Company Create(
        Guid tenantId,
        long companyNumber,
        int businessTypeId,
        string code,
        string name)
    {
        var now = DateTime.UtcNow;
        return new Company
        {
            CompanyGuid = Guid.NewGuid(),
            TenantId = tenantId,
            CompanyNumber = companyNumber,
            BusinessTypeId = businessTypeId,
            Code = code,
            Name = name,
            IsActive = true,
            CreatedOn = now,
            UpdatedOn = now
        };
    }
}
