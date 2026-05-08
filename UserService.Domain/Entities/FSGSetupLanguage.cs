namespace UserService.Domain.Entities;

public sealed class FSGSetupLanguage : AuditableEntity
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string CultureCode { get; private set; } = null!;
    public bool IsDefault { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }

    private FSGSetupLanguage()
    {
    }
}
