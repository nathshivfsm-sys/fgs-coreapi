using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Fgs.Setup.Infrastructure.Database;

public static class FgsSetupConnectionString
{
    public const string EnvironmentVariable = "FGS_SETUP_DB";

    public static string? Resolve(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.Resolve(
            configuration,
            ConnectionStringNames.FgsSetup,
            EnvironmentVariable,
            credentialProvider)
        ?? (string.IsNullOrWhiteSpace(configuration["Database:ConnectionString"])
            ? null
            : configuration["Database:ConnectionString"]);

    public static string ResolveRequired(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        Resolve(configuration, credentialProvider)
        ?? throw new InvalidOperationException(
            $"PostgreSQL connection string is required. Set ConnectionStrings:{ConnectionStringNames.FgsSetup}, "
            + $"Database:ConnectionString, {EnvironmentVariable}, or load DATABASE credentials from Setup Service.");

    public static string ResolveReadOnly(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.Resolve(
            configuration,
            ConnectionStringNames.FgsSetupReadOnly,
            "FGS_SETUP_DB_READONLY",
            credentialProvider)
        ?? ResolveRequired(configuration, credentialProvider);
}
