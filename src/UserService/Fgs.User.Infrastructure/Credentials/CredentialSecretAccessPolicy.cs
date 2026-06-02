using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Infrastructure.Common.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Credentials;

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
