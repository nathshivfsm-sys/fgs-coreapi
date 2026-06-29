using System.Data.Common;
using Fgs.Setup.Application.Abstractions.Persistence;
using Microsoft.Extensions.Configuration;

namespace Fgs.Setup.Infrastructure.Database.Read;

internal sealed class FgsSetupReadConnectionFactory : ISetupReadConnectionFactory
{
    private readonly string _connectionString;

    public FgsSetupReadConnectionFactory(IConfiguration configuration) =>
        _connectionString = FgsSetupConnectionString.ResolveReadOnly(configuration);

    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new Npgsql.NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
