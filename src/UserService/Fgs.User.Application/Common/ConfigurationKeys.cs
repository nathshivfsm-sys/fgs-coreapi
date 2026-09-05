namespace Fgs.User.Application.Common;

/// <summary>
/// Configuration section and key names used with <see cref="Microsoft.Extensions.Configuration.IConfiguration"/>.
/// </summary>
public static class ConfigurationKeys
{
    public static class EntraExternalId
    {
        public const string Section = "EntraExternalId";

        public const string RedirectUri = $"{Section}:RedirectUri";

        public const string LoginRedirectUri = $"{Section}:LoginRedirectUri";

        public const string UserFlow = $"{Section}:UserFlow";

        public const string PasswordUserFlow = $"{Section}:PasswordUserFlow";
    }

    public static class Application
    {
        public const string Section = "Application";

        /// <summary>
        /// Public gateway origin for the current environment (no trailing path),
        /// e.g. https://developer.fsm.com or http://100.54.14.213.
        /// </summary>
        public const string PublicBaseUrl = $"{Section}:PublicBaseUrl";

        /// <summary>
        /// Optional gateway path segment for service-prefixed URLs (EC2),
        /// e.g. <c>user-service</c> → <c>/user-service/api/v1/...</c>.
        /// Leave empty for local flat routes.
        /// </summary>
        public const string PublicServicePath = $"{Section}:PublicServicePath";

        /// <summary>
        /// SPA URL registered as the Entra External ID OAuth redirect URI
        /// (login + invite/signup), e.g. https://app.example.com/auth/callback.
        /// </summary>
        public const string UiAuthCallbackUrl = $"{Section}:UiAuthCallbackUrl";
    }

    public static class Invitation
    {
        public const string Section = "Invitation";

        public const string ExpiryDays = $"{Section}:ExpiryDays";

        public const string InviteBaseUrl = $"{Section}:InviteBaseUrl";
    }
}
