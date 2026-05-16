using System.Text.Json;
using Fgs.Platform.Application.IntegrationEvents;
using Fgs.Platform.Application.Notifications.Channels.Models;
using Fgs.Platform.Application.Notifications.Queues;
using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Infrastructure.Notifications.Queues;

public sealed class IntegrationEventMapper : IIntegrationEventMapper
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

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

    private static NotificationDispatchRequest MapCompanySignupInvite(
        string payload,
        string? correlationId,
        string messageId)
    {
        var evt = JsonSerializer.Deserialize<CompanySignupInviteEmailEvent>(payload, JsonOptions)!;
        return new NotificationDispatchRequest(
            evt.TenantId,
            NotificationChannel.Email,
            "CompanySignupInviteEmail",
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

    private static NotificationDispatchRequest MapUserInvited(
        string payload,
        string? correlationId,
        string messageId)
    {
        var evt = JsonSerializer.Deserialize<UserInvitedEvent>(payload, JsonOptions)!;
        return new NotificationDispatchRequest(
            evt.TenantId,
            NotificationChannel.Email,
            "UserInvited",
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
            NotificationChannel.Email,
            "PasswordReset",
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
            NotificationChannel.Email,
            "CompanyCreated",
            evt.AdminEmail,
            new Dictionary<string, string>
            {
                ["CompanyName"] = evt.CompanyName,
                ["AdminEmail"] = evt.AdminEmail
            },
            correlationId,
            messageId);
    }
}
