using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace Fgs.Foundation.Hosting;

public sealed class FgsApiHostOptions
{
    public string ServiceName { get; set; } = "fgs-service";

    public string? SwaggerTitle { get; set; }

    public string? SwaggerDescription { get; set; }

    public string? SwaggerContactName { get; set; }

    public Assembly? XmlCommentsAssembly { get; set; }

    public bool UseMultiTenancy { get; set; }

    public bool UseAuthenticationPipeline { get; set; } = true;

    public bool UseForwardedHeaders { get; set; }

    public bool UseTenantResolution { get; set; } = true;

    public bool UseResponseCompression { get; set; } = true;

    public bool UseActiveUserValidation { get; set; } = true;

    public Action<IApplicationBuilder>? PostAuthenticationMiddleware { get; set; }
}
