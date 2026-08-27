using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Fgs.Foundation.Api.Swagger;

internal static class TenantScopeSwaggerRules
{
    public static bool ShouldSkipTenantScopeHeaders(
        string? relativePath,
        MethodInfo methodInfo,
        IReadOnlyList<string> skipPathPrefixes)
    {
        if (HasAllowAnonymous(methodInfo))
        {
            return true;
        }

        var path = ResolveRequestPath(relativePath);
        return skipPathPrefixes.Any(prefix =>
            path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }

    public static IReadOnlyList<string> ResolveSkipPathPrefixes(IConfiguration configuration)
    {
        var configured = configuration
            .GetSection($"{FgsTenantScopeDefaults.ConfigurationSection}:{FgsTenantScopeDefaults.SkipPathPrefixesKey}")
            .Get<string[]>();

        return configured is { Length: > 0 }
            ? configured
            : FgsTenantScopeDefaults.SkipPathPrefixes;
    }

    internal static string ResolveRequestPath(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return string.Empty;
        }

        return relativePath.StartsWith('/') ? relativePath : $"/{relativePath}";
    }

    internal static bool HasAllowAnonymous(MethodInfo methodInfo)
    {
        if (methodInfo.GetCustomAttribute<AllowAnonymousAttribute>(inherit: true) is not null)
        {
            return true;
        }

        return methodInfo.DeclaringType?
            .GetCustomAttribute<AllowAnonymousAttribute>(inherit: true) is not null;
    }
}
