using System.Reflection;

namespace Fgs.Foundation.Api;

public sealed class FgsSwaggerOptions
{
    public const string ConfigurationSectionName = "Swagger";

    public required string Title { get; set; }

    public string? Description { get; set; }

    public string? ContactName { get; set; }

    public Assembly? XmlCommentsAssembly { get; set; }

    /// <summary>
    /// URL segment for Swagger UI and OpenAPI JSON (e.g. <c>swagger/user</c> behind the gateway).
    /// Defaults to <c>swagger</c>.
    /// </summary>
    public string RoutePrefix { get; set; } = "swagger";
}
