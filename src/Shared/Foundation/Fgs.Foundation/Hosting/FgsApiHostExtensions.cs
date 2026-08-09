using System.Reflection;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Compression.Extensions;
using Fgs.Foundation.Extensions;
using Fgs.Foundation.Idempotency;
using Fgs.MultiTenancy.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        builder.Services.AddFgsHttpIdempotency();
        builder.Services.AddControllers()
            .AddJsonOptions(json => json.JsonSerializerOptions.ConfigureFgsApi())
            .ConfigureApiBehaviorOptions(api =>
            {
                api.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(entry => entry.Value is { Errors.Count: > 0 })
                        .SelectMany(entry => entry.Value!.Errors.Select(error =>
                        {
                            if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
                            {
                                return error.ErrorMessage;
                            }

                            if (!string.IsNullOrWhiteSpace(error.Exception?.Message))
                            {
                                return error.Exception.Message;
                            }

                            return string.IsNullOrWhiteSpace(entry.Key)
                                ? "The request is invalid."
                                : $"{entry.Key} is invalid.";
                        }))
                        .Distinct()
                        .ToArray();

                    if (errors.Length == 0)
                    {
                        errors = ["The request is invalid."];
                    }

                    return new BadRequestObjectResult(
                        ApiResponse<object>.Fail(errors, StatusCodes.Status400BadRequest));
                };
            });
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

        if (options.UseResponseCompression)
        {
            builder.Services.AddFgsResponseCompression(builder.Configuration);
        }

        return options;
    }

    public static WebApplication UseFgsApiHost(
        this WebApplication app,
        FgsApiHostOptions options)
    {
        ApplyConfiguredHostOptions(app.Services, options);

        if (options.UseForwardedHeaders)
        {
            app.UseForwardedHeaders();
        }

        if (options.UseResponseCompression)
        {
            app.UseFgsResponseCompression();
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

            if (options.UseActiveUserValidation)
            {
                options.PostAuthenticationMiddleware?.Invoke(app);
            }

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

    internal static void ApplyConfiguredHostOptions(IServiceProvider services, FgsApiHostOptions options)
    {
        foreach (var configurator in services.GetServices<IConfigureOptions<FgsApiHostOptions>>())
        {
            configurator.Configure(options);
        }

        foreach (var postConfigurator in services.GetServices<IPostConfigureOptions<FgsApiHostOptions>>())
        {
            postConfigurator.PostConfigure(Options.DefaultName, options);
        }
    }
}
