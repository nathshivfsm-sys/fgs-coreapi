namespace Fgs.User.Application.Abstractions.Credentials;

public interface ICredentialSecretAccessPolicy
{
    bool IsSecretResolutionAllowed();
}
