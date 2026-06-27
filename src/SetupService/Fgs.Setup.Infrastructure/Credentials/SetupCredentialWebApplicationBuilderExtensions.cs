using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Setup.Infrastructure.Credentials;

public static class SetupCredentialWebApplicationBuilderExtensions
{
    public static async Task<WebApplicationBuilder> LoadFgsSetupCredentialsAsync(
        this WebApplicationBuilder builder,
        CancellationToken cancellationToken = default)
    {
        using var serviceProvider = builder.Services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        await scope.ServiceProvider
            .GetRequiredService<CredentialConfigurationLoader>()
            .ReloadAsync(cancellationToken);
        return builder;
    }
}
