using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Common;

/// <summary>
/// Resolves public gateway and UI URLs per environment.
/// Entra OAuth redirect uses <c>Application:UiAuthCallbackUrl</c> (SPA), not the API gateway path.
/// Optional <c>Application:PublicServicePath</c> inserts the Docker service name
/// (EC2: <c>/user-service/api/v1/...</c>; local: flat <c>/api/v1/...</c>) for API deep links only.
/// </summary>
public static class ApplicationPublicUrlResolver
{
    public static string ResolveUiAuthCallbackUrl(IConfiguration configuration) =>
        FirstNonEmpty(
            configuration[ConfigurationKeys.Application.UiAuthCallbackUrl],
            configuration["FGS_UI_AUTH_CALLBACK_URL"],
            Environment.GetEnvironmentVariable("FGS_UI_AUTH_CALLBACK_URL"),
            configuration[ConfigurationKeys.EntraExternalId.LoginRedirectUri],
            configuration[ConfigurationKeys.EntraExternalId.RedirectUri],
            ApplicationUrlDefaults.UiAuthCallback)!;

    /// <summary>
    /// Entra authorize/token redirect URI for login and invite/signup (SPA callback).
    /// </summary>
    public static string ResolveLoginRedirect(IConfiguration configuration) =>
        ResolveUiAuthCallbackUrl(configuration);

    public static string ResolveInviteBaseUrl(IConfiguration configuration) =>
        ResolveFromPublicBase(configuration, ApplicationUrlDefaults.InviteStartPath)
        ?? FirstNonEmpty(
            configuration[ConfigurationKeys.Invitation.InviteBaseUrl],
            ApplicationUrlDefaults.InviteStart)!;

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
