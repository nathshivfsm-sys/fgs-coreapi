namespace UserService.Domain.Entities;

public sealed class FSGSetupBusinessType : AuditableEntity
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<Company> Companies { get; private set; } = new List<Company>();

    private FSGSetupBusinessType()
    {
    }
}
