namespace Fgs.User.Application.Abstractions.Credentials;

/// <summary>
/// Resolves the current actor identifier for credential audit fields.
/// </summary>
public interface ICredentialActorResolver
{
    string ResolveActorId();
}
