namespace Fgs.User.Application.Abstractions.Credentials;

public interface ICredentialSecretNameBuilder
{
    /// <summary>
    /// Builds an AWS Secrets Manager name: {environment}/{applicationSlug}/{tenantCode}/{providerCode}.
    /// Example: prod/fsm/tenant-001/stripe
    /// </summary>
    string BuildSecretName(string environment, string tenantCode, string providerCode);
}
