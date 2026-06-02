namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// Routing keys published to and consumed from RabbitMQ topic exchanges.
/// </summary>
public static class IntegrationEventRoutingKeys
{
    public const string Prefix = "user.";

    public const string TenantProvisionRequested = "tenant.provision.requested";

    public const string CompanySignupInviteEmail = "user.CompanySignupInviteEmail";

    public const string UserInvited = "user.UserInvited";

    public const string PasswordReset = "user.PasswordReset";

    public const string CompanyCreated = "user.CompanyCreated";

    public const string CredentialConfigurationChanged = "user.CredentialConfigurationChanged";

    public static string ForEventType(string eventType, string? routingKeyPrefix = null) =>
        eventType switch
        {
            IntegrationEventTypes.TenantProvisionRequested => TenantProvisionRequested,
            IntegrationEventTypes.CompanySignupInviteEmail => CompanySignupInviteEmail,
            IntegrationEventTypes.UserInvited => UserInvited,
            IntegrationEventTypes.PasswordReset => PasswordReset,
            IntegrationEventTypes.CompanyCreated => CompanyCreated,
            IntegrationEventTypes.CredentialConfigurationChanged => CredentialConfigurationChanged,
            _ => $"{routingKeyPrefix ?? Prefix}{eventType}"
        };
}
