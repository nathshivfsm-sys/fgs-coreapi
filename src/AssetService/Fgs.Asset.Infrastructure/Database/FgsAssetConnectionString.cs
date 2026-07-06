using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Fgs.Asset.Infrastructure.Database;

public static class FgsAssetConnectionString
{
    public const string EnvironmentVariable = "FGS_ASSET_DB";

    public static string? Resolve(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.Resolve(
            configuration,
            ConnectionStringNames.FgsAsset,
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
            $"PostgreSQL connection string is required. Set ConnectionStrings:{ConnectionStringNames.FgsAsset}, "
            + $"Database:ConnectionString, {EnvironmentVariable}, or load DATABASE credentials from Asset Service.");

    public static string ResolveReadOnly(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.Resolve(
            configuration,
            ConnectionStringNames.FgsAssetReadOnly,
            "FGS_ASSET_DB_READONLY",
            credentialProvider)
        ?? ResolveRequired(configuration, credentialProvider);
}
