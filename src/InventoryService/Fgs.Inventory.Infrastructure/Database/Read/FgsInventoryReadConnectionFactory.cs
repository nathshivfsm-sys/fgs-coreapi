using System.Data.Common;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Microsoft.Extensions.Configuration;

namespace Fgs.Inventory.Infrastructure.Database.Read;

internal sealed class FgsInventoryReadConnectionFactory : IInventoryReadConnectionFactory
{
    private readonly string _connectionString;

    public FgsInventoryReadConnectionFactory(IConfiguration configuration) =>
        _connectionString = FgsInventoryConnectionString.ResolveReadOnly(configuration);

    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new Npgsql.NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
