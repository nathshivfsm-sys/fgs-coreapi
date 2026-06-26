using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Options;
using Fgs.Messaging.Serialization;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Fgs.Notification.Infrastructure.Notifications.Queues;

public sealed class IntegrationEventMapper(IOptions<NotificationOptions> notificationOptions)
    : IIntegrationEventMapper
{
    private static readonly JsonSerializerOptions JsonOptions = IntegrationEventJsonSerializerOptions.Create();

    private readonly NotificationOptions _notification = notificationOptions.Value;

    public bool CanMap(string routingKey) =>
        routingKey is IntegrationEventRoutingKeys.UserInvited
            or IntegrationEventRoutingKeys.PasswordReset
            or IntegrationEventRoutingKeys.CompanyCreated
            or IntegrationEventRoutingKeys.CompanySignupInviteEmail;

    public NotificationDispatchRequest? Map(
        string routingKey,
        string payload,
        string? correlationId,
        string messageId)
    {
        return routingKey switch
        {
            IntegrationEventRoutingKeys.CompanySignupInviteEmail => MapCompanySignupInvite(payload, correlationId, messageId),
            IntegrationEventRoutingKeys.UserInvited => MapUserInvited(payload, correlationId, messageId),
            IntegrationEventRoutingKeys.PasswordReset => MapPasswordReset(payload, correlationId, messageId),
            IntegrationEventRoutingKeys.CompanyCreated => MapCompanyCreated(payload, correlationId, messageId),
            _ => null
        };
    }

    private NotificationDispatchRequest MapCompanySignupInvite(
        string payload,
        string? correlationId,
        string messageId)
    {
        var evt = JsonSerializer.Deserialize<CompanySignupInviteEmailEvent>(payload, JsonOptions)!;
        var templateCode = string.IsNullOrWhiteSpace(evt.EmailTemplateCode)
            ? CommunicationTemplateCodes.CompanyAdminInvitation
            : evt.EmailTemplateCode;

        // Legacy payloads may carry a GUID string for CompanyId; those map to 0 and use global templates.
        return new NotificationDispatchRequest(
            evt.TenantId,
            evt.CompanyId > 0 ? evt.CompanyId : null,
            NotificationChannel.Email,
            templateCode,
            evt.Email,
            BuildCompanyAdminInvitationTokens(evt),
            correlationId,
            messageId);
    }

    private static NotificationDispatchRequest MapUserInvited(
        string payload,
        string? correlationId,
        string messageId)
    {
        var evt = JsonSerializer.Deserialize<UserInvitedEvent>(payload, JsonOptions)!;
        return new NotificationDispatchRequest(
            evt.TenantId,
            CompanyId: null,
            NotificationChannel.Email,
            "USER_INVITED",
            evt.Email,
            new Dictionary<string, string>
            {
                ["DisplayName"] = evt.DisplayName,
                ["InviteUrl"] = evt.InviteUrl,
                ["Email"] = evt.Email
            },
            correlationId,
            messageId);
    }

    private static NotificationDispatchRequest MapPasswordReset(
        string payload,
        string? correlationId,
        string messageId)
    {
        var evt = JsonSerializer.Deserialize<PasswordResetEvent>(payload, JsonOptions)!;
        return new NotificationDispatchRequest(
            evt.TenantId,
            CompanyId: null,
            NotificationChannel.Email,
            "PASSWORD_RESET",
            evt.Email,
            new Dictionary<string, string>
            {
                ["DisplayName"] = evt.DisplayName,
                ["ResetUrl"] = evt.ResetUrl,
                ["Email"] = evt.Email
            },
            correlationId,
            messageId);
    }

    private static NotificationDispatchRequest MapCompanyCreated(
        string payload,
        string? correlationId,
        string messageId)
    {
        var evt = JsonSerializer.Deserialize<CompanyCreatedEvent>(payload, JsonOptions)!;
        return new NotificationDispatchRequest(
            evt.TenantId,
            evt.CompanyId,
            NotificationChannel.Email,
            "COMPANY_CREATED",
            evt.AdminEmail,
            new Dictionary<string, string>
            {
                ["CompanyName"] = evt.CompanyName,
                ["AdminEmail"] = evt.AdminEmail
            },
            correlationId,
            messageId);
    }

    private Dictionary<string, string> BuildCompanyAdminInvitationTokens(CompanySignupInviteEmailEvent evt)
    {
        return new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["Name"] = FirstNonEmpty(evt.Name),
            ["PlatformName"] = FirstNonEmpty(evt.PlatformName, _notification.PlatformName),
            ["InviteLink"] = FirstNonEmpty(evt.InviteLink),
            ["ExpirationHours"] = FirstNonEmpty(evt.ExpirationHours, _notification.InvitationExpirationHours.ToString()),
            ["CompanyName"] = _notification.CompanyName,
            ["SupportEmail"] = FirstNonEmpty(evt.SupportEmail, _notification.SupportEmail),
            ["FgsTenantId"] = evt.TenantId.ToString()
        };
    }

    private static string FirstNonEmpty(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return string.Empty;
    }
}
