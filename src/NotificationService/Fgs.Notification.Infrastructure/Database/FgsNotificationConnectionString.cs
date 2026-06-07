using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Fgs.Notification.Infrastructure.Database;

public static class FgsNotificationConnectionString
{
    public const string Name = "FgsNotification";
    public const string EnvironmentVariable = "FGS_NOTIFICATION_DB";

    public static string ResolveRequired(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.ResolveRequired(configuration, Name, EnvironmentVariable, credentialProvider);
}
