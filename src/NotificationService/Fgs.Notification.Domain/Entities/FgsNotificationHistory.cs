using Fgs.Kernel.Entities;
using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Domain.Entities;

public sealed class FgsNotificationHistory : ITenantScoped
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
