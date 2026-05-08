namespace UserService.Domain.Entities;

public abstract class AuditableEntity
{
    public DateTime CreatedOn { get; protected set; }
    public Guid? CreatedBy { get; protected set; }
    public DateTime? UpdatedOn { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }

    protected AuditableEntity()
    {
    }
}
