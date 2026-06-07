using System.Reflection;
using Fgs.Foundation.Api;
using Fgs.Foundation.Extensions;
using Fgs.MultiTenancy.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Foundation.Hosting;

public static class FgsApiHostExtensions
{
    public static FgsApiHostOptions AddFgsApiHost(
        this WebApplicationBuilder builder,
        Action<FgsApiHostOptions> configure)
    {
        var options = new FgsApiHostOptions { ServiceName = "fgs-service" };
        configure(options);

        if (options.UseForwardedHeaders)
        {
            builder.Services.Configure<ForwardedHeadersOptions>(forwarded =>
            {
                forwarded.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        builder.Services.AddFgsApiVersioning();
        builder.Services.AddControllers()
            .AddJsonOptions(json => json.JsonSerializerOptions.ConfigureFgsApi());
        builder.Services.ConfigureHttpJsonOptions(json =>
            json.SerializerOptions.ConfigureFgsApi());
        builder.Services.AddFgsSwagger(swagger =>
        {
            swagger.Title = options.SwaggerTitle ?? options.ServiceName;
            swagger.Description = options.SwaggerDescription ?? string.Empty;
            swagger.ContactName = options.SwaggerContactName;
            swagger.XmlCommentsAssembly = options.XmlCommentsAssembly ?? Assembly.GetCallingAssembly();
        });

        if (options.UseMultiTenancy)
        {
            builder.Services.AddFgsMultiTenancy();
        }

        return options;
    }

    public static WebApplication UseFgsApiHost(
        this WebApplication app,
        FgsApiHostOptions options)
    {
        if (options.UseForwardedHeaders)
        {
            app.UseForwardedHeaders();
        }

        app.UseFgsFoundationMiddleware();

        if (FgsHostEnvironmentExtensions.ShouldUseHttpsRedirection(app.Configuration))
        {
            app.UseHttpsRedirection();
        }

        app.UseFgsSwagger();

        if (options.UseAuthenticationPipeline)
        {
            app.UseAuthentication();

            if (options.UseTenantResolution && options.UseMultiTenancy)
            {
                app.UseFgsTenantResolution();
            }

            app.UseAuthorization();
        }
        else if (options.UseTenantResolution && options.UseMultiTenancy)
        {
            app.UseFgsTenantResolution();
        }

        app.MapControllers();
        return app;
    }
}
