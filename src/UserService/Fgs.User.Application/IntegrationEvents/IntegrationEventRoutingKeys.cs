namespace Fgs.User.Application.IntegrationEvents;

/// <summary>
/// Routing keys published to <c>fgs.user</c> (topic exchange). Platform Service binds
/// <c>fgs.platform.notifications</c> to these keys for consumption.
/// </summary>
public static class IntegrationEventRoutingKeys
{
    public const string Prefix = "user.";

    public const string CompanySignupInviteEmail = "user.CompanySignupInviteEmail";

    public const string UserInvited = "user.UserInvited";

    public const string PasswordReset = "user.PasswordReset";

    public const string CompanyCreated = "user.CompanyCreated";

    public static string ForEventType(string eventType, string? routingKeyPrefix = null) =>
        $"{routingKeyPrefix ?? Prefix}{eventType}";
}
