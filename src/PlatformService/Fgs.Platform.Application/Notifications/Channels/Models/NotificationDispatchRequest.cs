using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Application.Notifications.Channels.Models;

public sealed record NotificationDispatchRequest(
    Guid TenantId,
    Guid? CompanyId,
    NotificationChannel Channel,
    string TemplateCode,
    string Recipient,
    IReadOnlyDictionary<string, string> TemplateData,
    string? CorrelationId,
    string? MessageId);
