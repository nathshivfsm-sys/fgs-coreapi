using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Domain.Entities;

public sealed class FgsNotificationHistory
{
    public Guid Id { get; set; }

    public long TenantId { get; set; }

    public NotificationChannel Channel { get; set; }

    public string TemplateName { get; set; } = string.Empty;

    public string? Recipient { get; set; }

    public NotificationDeliveryStatus Status { get; set; }

    public string? CorrelationId { get; set; }

    public string? ProviderMessageId { get; set; }

    public string? Error { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? SentOn { get; set; }
}
