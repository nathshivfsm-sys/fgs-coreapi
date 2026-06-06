using Microsoft.Extensions.Configuration;

namespace Fgs.Setup.Infrastructure.Database;

/// <summary>
/// Resolves PostgreSQL connection string for runtime and tooling (aligned with design-time factory).
/// </summary>
public static class FgsSetupConnectionString
{
    public const string ConfigurationKey = "FgsSetup";
    public const string EnvironmentVariable = "FGS_SETUP_DB";

    /// <summary>
    /// Order: <c>ConnectionStrings:FgsSetup</c>, <c>Database:ConnectionString</c>, environment variable <c>FGS_SETUP_DB</c>.
    /// </summary>
    public static string? Resolve(IConfiguration configuration)
    {
        static string? NullIfWhiteSpace(string? s) => string.IsNullOrWhiteSpace(s) ? null : s;

        return NullIfWhiteSpace(configuration.GetConnectionString(ConfigurationKey))
            ?? NullIfWhiteSpace(configuration["Database:ConnectionString"])
            ?? NullIfWhiteSpace(Environment.GetEnvironmentVariable(EnvironmentVariable));
    }

    /// <summary>
    /// Throws if no connection string is configured.
    /// </summary>
    public static string ResolveRequired(IConfiguration configuration) =>
        Resolve(configuration)
        ?? throw new InvalidOperationException(
            $"PostgreSQL connection string is required. Set ConnectionStrings:{ConfigurationKey}, " +
            $"or Database:ConnectionString, or environment variable {EnvironmentVariable}.");
}
