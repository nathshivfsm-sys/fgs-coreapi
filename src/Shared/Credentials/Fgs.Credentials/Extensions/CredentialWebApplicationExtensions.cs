using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Credentials.Extensions;

public static class CredentialWebApplicationExtensions
{
    public static async Task<WebApplication> LoadFgsRemoteCredentialsAsync(
        this WebApplication app,
        CancellationToken cancellationToken = default)
    {
        await app.Services.GetRequiredService<RemoteCredentialConfigurationLoader>().LoadAsync(cancellationToken);
        return app;
    }
}
