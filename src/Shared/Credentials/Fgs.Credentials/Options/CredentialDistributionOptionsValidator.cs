using Microsoft.Extensions.Options;

namespace Fgs.Credentials.Options;

public sealed class CredentialDistributionOptionsValidator : IValidateOptions<CredentialDistributionOptions>
{
    public ValidateOptionsResult Validate(string? name, CredentialDistributionOptions options)
    {
        if (!options.ValidateKeyOnStart)
        {
            return ValidateOptionsResult.Success;
        }

        if (!InternalServiceAuthorization.HasUsableConfiguredKey(options))
        {
            return ValidateOptionsResult.Fail(
                "CredentialDistribution:InternalServiceKey must be set. "
                + "Set CREDENTIAL_DISTRIBUTION_KEY or CredentialDistribution__InternalServiceKey.");
        }

        return ValidateOptionsResult.Success;
    }
}
