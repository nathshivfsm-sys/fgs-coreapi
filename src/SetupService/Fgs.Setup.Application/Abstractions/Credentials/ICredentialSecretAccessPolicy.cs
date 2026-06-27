namespace Fgs.Setup.Application.Abstractions.Credentials;

public interface ICredentialSecretAccessPolicy
{
    bool IsSecretResolutionAllowed();
}
