namespace Fgs.Kernel.Entities;

public interface ISoftDeletable
{
    bool IsActive { get; set; }
}
