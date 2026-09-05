using System.Data.Common;
using Fgs.Credentials.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Infrastructure.Database.Read;

internal sealed class FgsUserReadConnectionFactory(
    IConfiguration configuration,
    ICredentialConfigurationProvider? credentialProvider = null) : IUserReadConnectionFactory
{
    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = FgsUserConnectionString.ResolveReadOnly(configuration, credentialProvider);
        var connection = new Npgsql.NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
