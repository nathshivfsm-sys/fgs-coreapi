using Microsoft.Extensions.Configuration;

namespace Fgs.User.Infrastructure.Database;

/// <summary>
/// Resolves PostgreSQL connection string for runtime and tooling (aligned with design-time factory).
/// </summary>
public static class FgsUserConnectionString
{
    public const string ConfigurationKey = "FgsUser";
    public const string EnvironmentVariable = "FGS_USER_DB";

    /// <summary>
    /// Order: <c>ConnectionStrings:FgsUser</c>, <c>Database:ConnectionString</c>, environment variable <c>FGS_USER_DB</c>.
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
