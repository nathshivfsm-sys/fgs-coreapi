using System.Data.Common;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Fgs.Setup.Infrastructure.Provisioning;

internal sealed class TenantSeedDatabaseConnectionFactory : ITenantSeedDatabaseConnectionFactory
{
    private readonly Func<string> _baseConnectionStringFactory;
    private readonly IReadOnlyDictionary<string, string> _databaseConnectionStrings;

    public TenantSeedDatabaseConnectionFactory(
        Func<string> baseConnectionStringFactory,
        IOptions<TenantProvisioningOptions>? options = null)
    {
        _baseConnectionStringFactory = baseConnectionStringFactory
            ?? throw new ArgumentNullException(nameof(baseConnectionStringFactory));
        _databaseConnectionStrings = options?.Value.DatabaseConnectionStrings
            ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    public TenantSeedDatabaseConnectionFactory(
        string baseConnectionString,
        IOptions<TenantProvisioningOptions>? options = null)
        : this(() => baseConnectionString, options)
    {
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

        var builder = new NpgsqlConnectionStringBuilder(_baseConnectionStringFactory())
        {
            Database = databaseName
        };
        return builder.ConnectionString;
    }
}
