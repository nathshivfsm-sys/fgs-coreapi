using System.IO.Compression;
using Fgs.Foundation.Compression.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.Foundation.Compression.Extensions;

public static class ResponseCompressionExtensions
{
    private static readonly string[] DefaultMimeTypes =
    [
        "application/json",
        "application/problem+json",
        "text/plain",
        "application/xml"
    ];

    public static IServiceCollection AddFgsResponseCompression(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<FgsResponseCompressionOptions>(
            configuration.GetSection(FgsResponseCompressionOptions.SectionName));

        var options = configuration
                          .GetSection(FgsResponseCompressionOptions.SectionName)
                          .Get<FgsResponseCompressionOptions>()
                      ?? new FgsResponseCompressionOptions();

        if (!options.Enabled)
        {
            return services;
        }

        services.AddResponseCompression(compression =>
        {
            compression.EnableForHttps = options.EnableForHttps;
            compression.Providers.Add<BrotliCompressionProvider>();
            compression.Providers.Add<GzipCompressionProvider>();
            compression.MimeTypes = DefaultMimeTypes;
        });

        services.Configure<BrotliCompressionProviderOptions>(provider =>
            provider.Level = CompressionLevel.Fastest);

        services.Configure<GzipCompressionProviderOptions>(provider =>
            provider.Level = CompressionLevel.Fastest);

        return services;
    }

    public static IApplicationBuilder UseFgsResponseCompression(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetService<IOptions<FgsResponseCompressionOptions>>()?.Value;
        if (options is { Enabled: false })
        {
            return app;
        }

        return app.UseResponseCompression();
    }
}
