namespace Fgs.User.Application.Features.PublicEndpoints;

public static class PublicEndpointCodes
{
    public static readonly HashSet<string> EndpointTypes = new(StringComparer.Ordinal)
    {
        "BFF",
        "API"
    };

    public static readonly HashSet<string> EnvironmentCodes = new(StringComparer.Ordinal)
    {
        "PROD",
        "SANDBOX",
        "TRAINING",
        "QA",
        "PREVIEW",
        "DEVELOPMENT"
    };

    public static string Normalize(string value) => value.Trim().ToUpperInvariant();
}
