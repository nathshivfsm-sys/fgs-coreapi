using Fgs.Credentials;
using Fgs.Setup.Infrastructure.Credentials;
using Microsoft.Extensions.Configuration;

namespace Fgs.Setup.Infrastructure;

public static class CredentialConfigurationExtensions
{
    /// <summary>
    /// Binds decrypted credentials into <see cref="IConfiguration"/> under
    /// <c>ResolvedCredentials:*</c>, reading live from <paramref name="holder"/>.
    /// </summary>
    public static IConfigurationBuilder AddResolvedCredentialConfiguration(
        this IConfigurationBuilder builder,
        CredentialConfigurationHolder holder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(holder);
        return builder.Add(new CredentialConfigurationSource(holder));
    }
}
