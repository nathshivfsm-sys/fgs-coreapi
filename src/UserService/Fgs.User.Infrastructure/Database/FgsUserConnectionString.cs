using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Infrastructure.Database;

public static class FgsUserConnectionString
{
    public const string ConfigurationKey = "FgsUser";
    public const string EnvironmentVariable = "FGS_USER_DB";

    public static string? Resolve(IConfiguration configuration, ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.Resolve(
            configuration,
            ConfigurationKey,
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
            $"PostgreSQL connection string is required. Set ConnectionStrings:{ConfigurationKey}, {EnvironmentVariable}, or load DATABASE credentials from Setup Service.");
}
