using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Application.Notifications.Channels.Models;

public sealed record NotificationDispatchRequest(
    Guid TenantId,
    NotificationChannel Channel,
    string TemplateName,
    string Recipient,
    IReadOnlyDictionary<string, string> TemplateData,
    string? CorrelationId,
    string? MessageId);
