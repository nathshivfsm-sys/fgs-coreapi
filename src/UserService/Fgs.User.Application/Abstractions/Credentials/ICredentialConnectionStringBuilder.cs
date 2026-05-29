using Fgs.User.Application.Features.Credentials.Payloads;

namespace Fgs.User.Application.Abstractions.Credentials;

public interface ICredentialConnectionStringBuilder
{
    string BuildSqlConnectionString(SqlDatabaseSecretPayload payload);
}
