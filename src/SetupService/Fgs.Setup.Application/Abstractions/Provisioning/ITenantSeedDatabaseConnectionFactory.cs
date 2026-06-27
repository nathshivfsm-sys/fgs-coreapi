namespace Fgs.Setup.Application.Abstractions.Provisioning;

/// <summary>
/// Opens PostgreSQL connections for tenant seed operations, including cross-database copies.
/// </summary>
public interface ITenantSeedDatabaseConnectionFactory
{
    /// <summary>
    /// Uses <paramref name="configuredDatabaseName"/> when set; otherwise <paramref name="defaultDatabaseName"/>.
    /// </summary>
    string ResolveDatabaseName(string? configuredDatabaseName, string defaultDatabaseName);

    /// <summary>
    /// Opens a new connection to the resolved database. Caller owns and must dispose the connection.
    /// </summary>
    Task<System.Data.Common.DbConnection> OpenConnectionAsync(
        string databaseName,
        CancellationToken cancellationToken = default);
}
