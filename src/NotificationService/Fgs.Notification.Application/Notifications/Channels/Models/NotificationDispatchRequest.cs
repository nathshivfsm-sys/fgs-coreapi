using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Notifications.Channels.Models;

public sealed record NotificationDispatchRequest(
    long TenantId,
    long? CompanyId,
    NotificationChannel Channel,
    string TemplateCode,
    string Recipient,
    IReadOnlyDictionary<string, string> TemplateData,
    string? CorrelationId,
    string? MessageId);
