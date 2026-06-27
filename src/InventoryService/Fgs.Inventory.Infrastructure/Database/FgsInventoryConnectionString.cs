using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Fgs.Inventory.Infrastructure.Database;

public static class FgsInventoryConnectionString
{
    public const string EnvironmentVariable = "FGS_INVENTORY_DB";

    public static string? Resolve(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.Resolve(
            configuration,
            ConnectionStringNames.FgsInventory,
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
            $"PostgreSQL connection string is required. Set ConnectionStrings:{ConnectionStringNames.FgsInventory}, "
            + $"Database:ConnectionString, {EnvironmentVariable}, or load DATABASE credentials from Inventory Service.");

    public static string ResolveReadOnly(
        IConfiguration configuration,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        ConnectionStringResolver.Resolve(
            configuration,
            ConnectionStringNames.FgsInventory,
            "FGS_INVENTORY_DB_READONLY",
            credentialProvider)
        ?? ResolveRequired(configuration, credentialProvider);
}
