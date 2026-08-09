using Fgs.Setup.Application.Abstractions.Credentials;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Fgs.Credentials.Options;

namespace Fgs.Setup.Infrastructure.Credentials;

public sealed class CredentialSecretAccessPolicy : ICredentialSecretAccessPolicy
{
    private readonly IHostEnvironment _environment;
    private readonly AwsCredentialsOptions _options;

    public CredentialSecretAccessPolicy(IHostEnvironment environment, IOptions<AwsCredentialsOptions> options)
    {
        _environment = environment;
        _options = options.Value;
    }

    public bool IsSecretResolutionAllowed() =>
        _environment.IsDevelopment() && _options.EnableTestSecretEndpoint;
}
