using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Infrastructure.Database;

public static class FgsUserConnectionString
{
    public const string EnvironmentVariable = "FGS_USER_DB";

    public static string? Resolve(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.Resolve(
            configuration,
            ConnectionStringNames.FgsUser,
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
            $"PostgreSQL connection string is required. Set ConnectionStrings:{ConnectionStringNames.FgsUser}, "
            + $"Database:ConnectionString, {EnvironmentVariable}, or load DATABASE credentials from Setup Service.");

    public static string ResolveReadOnly(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.Resolve(
            configuration,
            ConnectionStringNames.FgsUserReadOnly,
            "FGS_USER_DB_READONLY",
            credentialProvider)
        ?? ResolveRequired(configuration, credentialProvider);
}
