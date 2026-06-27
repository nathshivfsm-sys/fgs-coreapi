using System.Data.Common;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Microsoft.Extensions.Options;
using Npgsql;
using Fgs.Setup.Infrastructure.Common.Options;

namespace Fgs.Setup.Infrastructure.Provisioning;

internal sealed class TenantSeedDatabaseConnectionFactory : ITenantSeedDatabaseConnectionFactory
{
    private readonly string _baseConnectionString;
    private readonly IReadOnlyDictionary<string, string> _databaseConnectionStrings;

    public TenantSeedDatabaseConnectionFactory(
        string baseConnectionString,
        IOptions<TenantProvisioningOptions>? options = null)
    {
        _baseConnectionString = baseConnectionString;
        _databaseConnectionStrings = options?.Value.DatabaseConnectionStrings
            ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    public string ResolveDatabaseName(string? configuredDatabaseName, string defaultDatabaseName) =>
        string.IsNullOrWhiteSpace(configuredDatabaseName)
            ? defaultDatabaseName
            : configuredDatabaseName;

    public async Task<DbConnection> OpenConnectionAsync(
        string databaseName,
        CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(BuildConnectionString(databaseName));
        await connection.OpenAsync(cancellationToken);
        return connection;
    }

    internal string BuildConnectionString(string databaseName)
    {
        if (_databaseConnectionStrings.TryGetValue(databaseName, out var overrideConnectionString)
            && !string.IsNullOrWhiteSpace(overrideConnectionString))
        {
            return overrideConnectionString;
        }

        var builder = new NpgsqlConnectionStringBuilder(_baseConnectionString)
        {
            Database = databaseName
        };
        return builder.ConnectionString;
    }
}
