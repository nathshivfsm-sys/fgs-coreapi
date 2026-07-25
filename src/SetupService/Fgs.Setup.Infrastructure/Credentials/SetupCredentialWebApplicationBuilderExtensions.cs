using Fgs.Credentials.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Setup.Infrastructure.Credentials;

public static class SetupCredentialWebApplicationBuilderExtensions
{
    public static async Task<WebApplicationBuilder> LoadFgsSetupCredentialsAsync(
        this WebApplicationBuilder builder,
        CancellationToken cancellationToken = default)
    {
        await using var serviceProvider = builder.Services.BuildServiceProvider();
        await serviceProvider
            .GetRequiredService<ICredentialConfigurationProvider>()
            .ReloadAsync(cancellationToken);
        return builder;
    }
}
