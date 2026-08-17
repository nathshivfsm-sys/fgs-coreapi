using System.Data.Common;
using Fgs.Credentials.Abstractions;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Microsoft.Extensions.Configuration;

namespace Fgs.Inventory.Infrastructure.Database.Read;

internal sealed class FgsInventoryReadConnectionFactory(
    IConfiguration configuration,
    ICredentialConfigurationProvider? credentialProvider = null) : IInventoryReadConnectionFactory
{
    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = FgsInventoryConnectionString.ResolveReadOnly(configuration, credentialProvider);
        var connection = new Npgsql.NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
