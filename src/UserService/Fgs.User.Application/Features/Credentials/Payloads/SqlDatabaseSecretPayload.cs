namespace Fgs.User.Application.Features.Credentials.Payloads;

public sealed class SqlDatabaseSecretPayload
{
    public string Server { get; init; } = null!;

    public string Database { get; init; } = null!;

    public string Username { get; init; } = null!;

    public string Password { get; init; } = null!;
}
