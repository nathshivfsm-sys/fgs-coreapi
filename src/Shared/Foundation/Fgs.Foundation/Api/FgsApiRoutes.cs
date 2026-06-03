namespace Fgs.Foundation.Api;

public static class FgsApiRoutes
{
    public const string VersionParameter = "{version:apiVersion}";

    public static string Versioned(string path) =>
        $"api/v{VersionParameter}/{Normalize(path)}";

    public static string UrlPrefix => FgsApiVersions.UrlPrefix;

    private static string Normalize(string path) =>
        path.Trim().TrimStart('/').TrimEnd('/');
}
