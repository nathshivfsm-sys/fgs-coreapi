using Fgs.User.Application.Abstractions.Credentials;
using Microsoft.Extensions.Options;
using Fgs.User.Infrastructure.Common.Options;

namespace Fgs.User.Infrastructure.Secrets;

public sealed class CredentialSecretNameBuilder(IOptions<AwsCredentialsOptions> options)
    : ICredentialSecretNameBuilder
{
    public string BuildSecretName(string environment, string tenantCode, string providerCode)
    {
        var applicationSlug = options.Value.ApplicationSlug.Trim('/');
        var env = Sanitize(environment).ToLowerInvariant();
        var tenant = Sanitize(tenantCode).ToLowerInvariant();
        var provider = Sanitize(providerCode).ToLowerInvariant();

        return $"{env}/{applicationSlug}/{tenant}/{provider}";
    }

    private static string Sanitize(string value) =>
        value.Trim()
            .Replace("/", "-", StringComparison.Ordinal)
            .Replace(":", "-", StringComparison.Ordinal)
            .Replace(" ", "-", StringComparison.Ordinal);
}
