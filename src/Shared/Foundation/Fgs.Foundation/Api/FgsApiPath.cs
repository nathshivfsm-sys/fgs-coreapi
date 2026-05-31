using Microsoft.AspNetCore.Http;

namespace Fgs.Foundation.Api;

public static class FgsApiPath
{
    public static bool StartsWithApiArea(PathString path, string area) =>
        path.StartsWithSegments(BuildAreaPath(area), StringComparison.OrdinalIgnoreCase);

    public static string BuildAreaPath(string area) =>
        $"/api/{FgsApiVersions.UrlPrefix}/{area.Trim().TrimStart('/')}";
}
