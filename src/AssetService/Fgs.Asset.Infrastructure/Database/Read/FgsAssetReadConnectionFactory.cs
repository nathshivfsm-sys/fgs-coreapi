using System.Data.Common;
using Fgs.Asset.Application.Abstractions.Persistence;
using Microsoft.Extensions.Configuration;

namespace Fgs.Asset.Infrastructure.Database.Read;

internal sealed class FgsAssetReadConnectionFactory : IAssetReadConnectionFactory
{
    private readonly string _connectionString;

    public FgsAssetReadConnectionFactory(IConfiguration configuration) =>
        _connectionString = FgsAssetConnectionString.ResolveReadOnly(configuration);

    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new Npgsql.NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
