using Microsoft.Extensions.Configuration;

namespace Fgs.User.Infrastructure.Database;

public static class FgsUserConnectionString
{
    public const string ConfigurationKey = "FgsUser";
    public const string EnvironmentVariable = "FGS_USER_DB";

    public static string? Resolve(IConfiguration configuration)
    {
        static string? NullIfWhiteSpace(string? s) => string.IsNullOrWhiteSpace(s) ? null : s;

        return NullIfWhiteSpace(configuration.GetConnectionString(ConfigurationKey))
            ?? NullIfWhiteSpace(configuration["Database:ConnectionString"])
            ?? NullIfWhiteSpace(Environment.GetEnvironmentVariable(EnvironmentVariable));
    }

    public static string ResolveRequired(IConfiguration configuration) =>
        Resolve(configuration)
        ?? throw new InvalidOperationException(
            $"PostgreSQL connection string is required. Set ConnectionStrings:{ConfigurationKey} or {EnvironmentVariable}.");
}
