namespace Fgs.Kernel.Events;

public interface IDomainEvent
{
    Guid EventId { get; }

    DateTimeOffset OccurredOn { get; }
}
