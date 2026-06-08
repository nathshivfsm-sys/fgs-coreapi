using Microsoft.Extensions.Configuration;

namespace Fgs.Foundation.Hosting;

public static class FgsHostEnvironmentExtensions
{
    public static bool ShouldUseHttpsRedirection(IConfiguration configuration) =>
        !string.Equals(configuration["DOTNET_RUNNING_IN_CONTAINER"], "true", StringComparison.OrdinalIgnoreCase)
        && (configuration["ASPNETCORE_URLS"]?.Contains("https://", StringComparison.OrdinalIgnoreCase) == true
            || !string.IsNullOrWhiteSpace(configuration["ASPNETCORE_HTTPS_PORT"]));
}
