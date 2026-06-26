namespace Fgs.Notification.Domain.Entities;

public sealed class FgsProcessedIntegrationEvent
{
    public Guid Id { get; set; }

    public string MessageId { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;

    public DateTimeOffset ProcessedOn { get; set; }
}
