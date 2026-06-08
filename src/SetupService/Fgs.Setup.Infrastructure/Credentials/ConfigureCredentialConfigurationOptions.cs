using Fgs.Credentials;
using Fgs.Setup.Application.Common.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Infrastructure.Credentials;

internal sealed class ConfigureCredentialConfigurationOptions : IConfigureOptions<CredentialConfigurationOptions>
{
    private readonly CredentialConfigurationHolder _holder;

    public ConfigureCredentialConfigurationOptions(CredentialConfigurationHolder holder) => _holder = holder;

    public void Configure(CredentialConfigurationOptions options)
    {
        options.Values = _holder.Values.ToDictionary(
            static entry => entry.Key,
            static entry => entry.Value,
            StringComparer.OrdinalIgnoreCase);
    }
}
