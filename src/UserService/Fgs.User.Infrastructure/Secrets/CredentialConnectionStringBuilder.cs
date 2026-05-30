using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Features.Credentials.Payloads;
using Npgsql;

namespace Fgs.User.Infrastructure.Secrets;

public sealed class CredentialConnectionStringBuilder : ICredentialConnectionStringBuilder
{
    public string BuildSqlConnectionString(SqlDatabaseSecretPayload payload)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = payload.Server,
            Database = payload.Database,
            Username = payload.Username,
            Password = payload.Password
        };

        return builder.ConnectionString;
    }
}
