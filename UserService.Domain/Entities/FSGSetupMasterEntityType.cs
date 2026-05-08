namespace UserService.Domain.Entities;

public sealed class FSGSetupMasterEntityType : AuditableEntity
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }

    private FSGSetupMasterEntityType()
    {
    }
}
