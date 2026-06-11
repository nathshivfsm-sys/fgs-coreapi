using Fgs.Persistence.CatalogCrud;
using Fgs.Setup.Infrastructure.Database.Read;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Fgs.Setup.Infrastructure.Database.Read;

public sealed class FgsSetupReadConnectionFactory : ICatalogReadConnectionFactory
{
    private readonly string _connectionString;

    public FgsSetupReadConnectionFactory(IConfiguration configuration) =>
        _connectionString = FgsSetupConnectionString.ResolveReadOnly(configuration);

    public async Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
