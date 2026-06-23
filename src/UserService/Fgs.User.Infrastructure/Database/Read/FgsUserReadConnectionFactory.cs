using System.Data.Common;
using Fgs.User.Application.Abstractions.Persistence;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Infrastructure.Database.Read;

internal sealed class FgsUserReadConnectionFactory : IUserReadConnectionFactory
{
    private readonly string _connectionString;

    public FgsUserReadConnectionFactory(IConfiguration configuration) =>
        _connectionString = FgsUserConnectionString.ResolveReadOnly(configuration);

    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new Npgsql.NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
