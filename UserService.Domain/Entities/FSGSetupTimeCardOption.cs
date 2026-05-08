namespace UserService.Domain.Entities;

public sealed class FSGSetupTimeCardOption : AuditableEntity
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private FSGSetupTimeCardOption()
    {
    }
}
