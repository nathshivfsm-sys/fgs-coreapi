using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Fgs.File.Infrastructure.Database;

public static class FgsFileConnectionString
{
    public const string ConfigurationKey = "FgsFile";
    public const string EnvironmentVariable = "FGS_FILE_DB";

    public static string ResolveRequired(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.ResolveRequired(
            configuration,
            ConfigurationKey,
            EnvironmentVariable,
            credentialProvider);
}
