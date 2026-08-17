using System.Data.Common;
using Fgs.Credentials.Abstractions;
using Fgs.Setup.Application.Abstractions.Persistence;
using Microsoft.Extensions.Configuration;

namespace Fgs.Setup.Infrastructure.Database.Read;

internal sealed class FgsSetupReadConnectionFactory(
    IConfiguration configuration,
    ICredentialConfigurationProvider? credentialProvider = null) : ISetupReadConnectionFactory
{
    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = FgsSetupConnectionString.ResolveReadOnly(configuration, credentialProvider);
        var connection = new Npgsql.NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
