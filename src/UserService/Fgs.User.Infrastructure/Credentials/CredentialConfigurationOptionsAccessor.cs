using Fgs.User.Application.Common.Options;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Credentials;

public sealed class CredentialConfigurationOptionsAccessor : IOptions<CredentialConfigurationOptions>
{
    private readonly CredentialConfigurationHolder _holder;

    public CredentialConfigurationOptionsAccessor(CredentialConfigurationHolder holder) => _holder = holder;

    public CredentialConfigurationOptions Value => new()
    {
        Values = _holder.Values.ToDictionary(
            static entry => entry.Key,
            static entry => entry.Value,
            StringComparer.OrdinalIgnoreCase)
    };
}
