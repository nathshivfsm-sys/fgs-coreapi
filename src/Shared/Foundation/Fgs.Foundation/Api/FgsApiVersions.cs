using Asp.Versioning;

namespace Fgs.Foundation.Api;

public static class FgsApiVersions
{
    public const string V1 = "1.0";

    /// <summary>URL segment for v1 routes (e.g. <c>/api/v1/auth</c>).</summary>
    public const string UrlPrefix = "v1";

    public static ApiVersion Default { get; } = new(1, 0);
}
