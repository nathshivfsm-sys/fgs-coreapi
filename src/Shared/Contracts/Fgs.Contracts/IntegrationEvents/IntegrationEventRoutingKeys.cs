namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// Routing keys published to and consumed from RabbitMQ topic exchanges.
/// </summary>
public static class IntegrationEventRoutingKeys
{
    public const string Prefix = "user.";

    public const string TenantProvisionRequested = "tenant.provision.requested";

    public const string TenantProvisionCompleted = "tenant.provision.completed";

    public const string CompanySignupInviteEmail = "user.CompanySignupInviteEmail";

    public const string UserInvited = "user.UserInvited";

    public const string PasswordReset = "user.PasswordReset";

    public const string CompanyCreated = "user.CompanyCreated";

    public const string CredentialConfigurationChanged = "setup.CredentialConfigurationChanged";

    public const string CredentialAuditRequested = "audit.credential.requested";

    public const string InventoryStockChanged = "inventory.InventoryStockChanged";

    public const string PurchaseOrderStatusChanged = "inventory.PurchaseOrderStatusChanged";

    public static string ForEventType(string eventType, string? routingKeyPrefix = null) =>
        eventType switch
        {
            IntegrationEventTypes.TenantProvisionRequested => TenantProvisionRequested,
            IntegrationEventTypes.TenantProvisionCompleted => TenantProvisionCompleted,
            IntegrationEventTypes.CompanySignupInviteEmail => CompanySignupInviteEmail,
            IntegrationEventTypes.UserInvited => UserInvited,
            IntegrationEventTypes.PasswordReset => PasswordReset,
            IntegrationEventTypes.CompanyCreated => CompanyCreated,
            IntegrationEventTypes.CredentialConfigurationChanged => CredentialConfigurationChanged,
            IntegrationEventTypes.CredentialAuditRequested => CredentialAuditRequested,
            IntegrationEventTypes.InventoryStockChanged => InventoryStockChanged,
            IntegrationEventTypes.PurchaseOrderStatusChanged => PurchaseOrderStatusChanged,
            _ => $"{routingKeyPrefix ?? Prefix}{eventType}"
        };
}
