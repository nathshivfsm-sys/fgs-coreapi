namespace Fgs.User.Domain.Entities;

public class FgsCredentialProvider : FgsEntityBase
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid CompanyId { get; set; }

    public int CredentialProviderTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Environment { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
