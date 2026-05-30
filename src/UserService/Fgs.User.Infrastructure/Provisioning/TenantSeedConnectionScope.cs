using System.Data;
using System.Data.Common;
using Fgs.User.Application.Abstractions.Provisioning;

namespace Fgs.User.Infrastructure.Provisioning;

/// <summary>
/// Leases database connections for a seed run. Disposes only connections it opened.
/// </summary>
internal sealed class TenantSeedConnectionScope : IAsyncDisposable
{
    private readonly ITenantSeedDatabaseConnectionFactory _connectionFactory;
    private readonly DbConnection _primaryConnection;
    private readonly string _defaultDatabaseName;
    private readonly Dictionary<string, DbConnection> _connections = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _ownedDatabaseNames = new(StringComparer.OrdinalIgnoreCase);

    public TenantSeedConnectionScope(
        ITenantSeedDatabaseConnectionFactory connectionFactory,
        DbConnection primaryConnection,
        string defaultDatabaseName)
    {
        _connectionFactory = connectionFactory;
        _primaryConnection = primaryConnection;
        _defaultDatabaseName = defaultDatabaseName;
        _connections[defaultDatabaseName] = primaryConnection;
    }

    public string DefaultDatabaseName => _defaultDatabaseName;

    public string ResolveSourceDatabaseName(string? configuredDatabaseName) =>
        _connectionFactory.ResolveDatabaseName(configuredDatabaseName, _defaultDatabaseName);

    public string ResolveTargetDatabaseName(string? configuredDatabaseName) =>
        _connectionFactory.ResolveDatabaseName(configuredDatabaseName, _defaultDatabaseName);

    public async Task<DbConnection> GetConnectionAsync(string databaseName, CancellationToken cancellationToken)
    {
        if (_connections.TryGetValue(databaseName, out var existing))
        {
            if (existing.State != ConnectionState.Open)
            {
                await existing.OpenAsync(cancellationToken);
            }

            return existing;
        }

        var opened = await _connectionFactory.OpenConnectionAsync(databaseName, cancellationToken);
        _connections[databaseName] = opened;
        _ownedDatabaseNames.Add(databaseName);
        return opened;
    }

    public async Task<(DbConnection Source, DbConnection Target)> GetSourceAndTargetConnectionsAsync(
        string? sourceDatabaseName,
        string? targetDatabaseName,
        CancellationToken cancellationToken)
    {
        var sourceDb = ResolveSourceDatabaseName(sourceDatabaseName);
        var targetDb = ResolveTargetDatabaseName(targetDatabaseName);
        var source = await GetConnectionAsync(sourceDb, cancellationToken);
        var target = await GetConnectionAsync(targetDb, cancellationToken);
        return (source, target);
    }

    public bool IsCrossDatabase(string? sourceDatabaseName, string? targetDatabaseName)
    {
        var sourceDb = ResolveSourceDatabaseName(sourceDatabaseName);
        var targetDb = ResolveTargetDatabaseName(targetDatabaseName);
        return !string.Equals(sourceDb, targetDb, StringComparison.OrdinalIgnoreCase);
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var databaseName in _ownedDatabaseNames)
        {
            if (_connections.TryGetValue(databaseName, out var connection))
            {
                await connection.DisposeAsync();
            }
        }
    }
}
