using System.Data.Common;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Fgs.Asset.Infrastructure.Database.Read;

internal sealed class FgsAssetReadConnectionFactory(
    IConfiguration configuration,
    ICredentialConfigurationProvider? credentialProvider = null) : IAssetReadConnectionFactory
{
    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = FgsAssetConnectionString.ResolveReadOnly(configuration, credentialProvider);
        var connection = new Npgsql.NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
