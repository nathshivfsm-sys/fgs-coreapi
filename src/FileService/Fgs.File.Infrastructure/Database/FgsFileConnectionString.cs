using Microsoft.Extensions.Configuration;

namespace Fgs.File.Infrastructure.Database;

public static class FgsFileConnectionString
{
    public const string ConfigurationKey = "FgsFile";
    public const string EnvironmentVariable = "FGS_FILE_DB";

    public static string ResolveRequired(IConfiguration configuration) =>
        configuration.GetConnectionString(ConfigurationKey)
        ?? Environment.GetEnvironmentVariable(EnvironmentVariable)
        ?? throw new InvalidOperationException($"Connection string {ConfigurationKey} is required.");
}
