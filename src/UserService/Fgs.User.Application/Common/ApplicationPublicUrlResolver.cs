using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Common;

/// <summary>
/// Resolves public gateway URLs per environment.
/// Prefer <c>Application:PublicBaseUrl</c> (compose/env) over credential-backed Entra RedirectUri
/// so local vs EC2 do not share a hardcoded localhost callback.
/// Optional <c>Application:PublicServicePath</c> inserts the Docker service name
/// (EC2: <c>/user-service/api/v1/...</c>; local: flat <c>/api/v1/...</c>).
/// </summary>
public static class ApplicationPublicUrlResolver
{
    public static string ResolveEntraCallbackRedirect(IConfiguration configuration) =>
        ResolveFromPublicBase(configuration, ApplicationUrlDefaults.EntraCallbackPath)
        ?? FirstNonEmpty(
            configuration[ConfigurationKeys.EntraExternalId.RedirectUri],
            ApplicationUrlDefaults.EntraCallbackRedirect)!;

    public static string ResolveLoginRedirect(IConfiguration configuration) =>
        ResolveFromPublicBase(configuration, ApplicationUrlDefaults.EntraCallbackPath)
        ?? FirstNonEmpty(
            configuration[ConfigurationKeys.EntraExternalId.LoginRedirectUri],
            configuration[ConfigurationKeys.EntraExternalId.RedirectUri],
            ApplicationUrlDefaults.EntraCallbackRedirect)!;

    public static string ResolveInviteBaseUrl(IConfiguration configuration) =>
        ResolveFromPublicBase(configuration, ApplicationUrlDefaults.InviteStartPath)
        ?? FirstNonEmpty(
            configuration[ConfigurationKeys.Invitation.InviteBaseUrl],
            ApplicationUrlDefaults.InviteStart)!;

    public static string ResolveDashboardUrl(IConfiguration configuration) =>
        ResolveFromPublicBase(configuration, ApplicationUrlDefaults.DashboardPath)
        ?? FirstNonEmpty(
            configuration[ConfigurationKeys.Application.DashboardUrl],
            ApplicationUrlDefaults.Dashboard)!;

    private static string? ResolveFromPublicBase(IConfiguration configuration, string path)
    {
        var publicBase = FirstNonEmpty(
            configuration[ConfigurationKeys.Application.PublicBaseUrl],
            configuration["FGS_PUBLIC_BASE_URL"],
            Environment.GetEnvironmentVariable("FGS_PUBLIC_BASE_URL"));

        if (string.IsNullOrWhiteSpace(publicBase))
        {
            return null;
        }

        var servicePath = NormalizeServicePath(
            FirstNonEmpty(
                configuration[ConfigurationKeys.Application.PublicServicePath],
                configuration["FGS_PUBLIC_SERVICE_PATH"],
                Environment.GetEnvironmentVariable("FGS_PUBLIC_SERVICE_PATH")));

        var relative = path.TrimStart('/');
        if (!string.IsNullOrEmpty(servicePath))
        {
            relative = $"{servicePath}/{relative}";
        }

        return $"{publicBase.TrimEnd('/')}/{relative}";
    }

    private static string? NormalizeServicePath(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim().Trim('/');
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return null;
    }
}
