using Fgs.Foundation.Api;
using Fgs.Foundation.Api.Swagger;
using Fgs.Foundation.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fgs.Foundation.Tests.Swagger;

public sealed class ConfigureSwaggerGenOptionsTests
{
    [Fact]
    public void AddFgsSwagger_RegistersTenantScopeSwaggerOperationFilter()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        services.AddFgsApiVersioning();
        services.AddFgsSwagger(options =>
        {
            options.Title = "FGS Test Service";
            options.RoutePrefix = "swagger/test";
        });

        using var provider = services.BuildServiceProvider();
        var configureOptions = provider.GetServices<IConfigureOptions<SwaggerGenOptions>>()
            .OfType<ConfigureSwaggerGenOptions>()
            .ToArray();

        configureOptions.Should().NotBeEmpty();

        var swaggerGenOptions = new SwaggerGenOptions();
        foreach (var configure in provider.GetServices<IConfigureOptions<SwaggerGenOptions>>())
        {
            configure.Configure(swaggerGenOptions);
        }

        swaggerGenOptions.OperationFilterDescriptors
            .Select(descriptor => descriptor.Type)
            .Should()
            .Contain(typeof(TenantScopeSwaggerOperationFilter));
    }
}
