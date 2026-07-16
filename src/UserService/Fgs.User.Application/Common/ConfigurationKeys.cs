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
    }

    public static class Application
    {
        public const string Section = "Application";

        public const string DashboardUrl = $"{Section}:DashboardUrl";
    }

    public static class Invitation
    {
        public const string Section = "Invitation";

        public const string ExpiryDays = $"{Section}:ExpiryDays";

        public const string InviteBaseUrl = $"{Section}:InviteBaseUrl";
    }
}
