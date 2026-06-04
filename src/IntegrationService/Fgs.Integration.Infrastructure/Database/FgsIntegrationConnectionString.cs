using Microsoft.Extensions.Configuration;

namespace Fgs.Integration.Infrastructure.Database;

public static class FgsIntegrationConnectionString
{
    public const string Name = "FgsIntegration";

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
