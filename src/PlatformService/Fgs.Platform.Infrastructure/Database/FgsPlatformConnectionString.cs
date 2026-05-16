using Microsoft.Extensions.Configuration;

namespace Fgs.Platform.Infrastructure.Database;

public static class FgsPlatformConnectionString
{
    public const string Name = "FgsPlatform";

    public static string ResolveRequired(IConfiguration configuration)
    {
        var value = configuration.GetConnectionString(Name);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"Connection string '{Name}' is not configured.");
        }

        return value;
    }
}
