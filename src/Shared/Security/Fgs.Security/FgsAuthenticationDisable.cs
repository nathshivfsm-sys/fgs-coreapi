using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Fgs.Security;

/// <summary>
/// Dev-only switch to skip JWT / fallback auth. Never honored when environment is Production.
/// Config: <c>Authentication:DisableTokenValidation=true</c>
/// </summary>
public static class FgsAuthenticationDisable
{
    public const string ConfigurationKey = "Authentication:DisableTokenValidation";

    public static bool IsEnabled(IConfiguration configuration)
    {
        if (!configuration.GetValue(ConfigurationKey, false))
        {
            return false;
        }

        var env = configuration["ASPNETCORE_ENVIRONMENT"]
                  ?? configuration[HostDefaults.EnvironmentKey]
                  ?? Environments.Production;

        return !string.Equals(env, Environments.Production, StringComparison.OrdinalIgnoreCase);
    }
}
