using Fgs.Bff.API.GraphQL;
using Fgs.Bff.Application;
using Fgs.Bff.Infrastructure;
using Fgs.Credentials;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;
using Serilog;

// Non-reloadable logger: LoadFgsRemoteCredentialsAsync builds a temporary ServiceProvider
// before WebApplication.Build(), which freezes Serilog's CreateBootstrapLogger().
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.ApplyFgsKmsEnvironmentVariable();

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithProperty("Service", "fgs-bff-service")
        .WriteTo.Console());

    var hostOptions = builder.AddFgsApiHost(options =>
    {
        options.ServiceName = "fgs-bff-service";
        options.SwaggerTitle = "FGS BFF Service";
        options.SwaggerDescription =
            "Backend-for-Frontend: authentication, authorization, orchestration, DTO mapping, "
            + "and response aggregation for cross-domain workflows. "
            + "Simple CRUD continues to route directly from NGINX to owning microservices.";
        options.SwaggerContactName = "FGS Platform";
        options.XmlCommentsAssembly = typeof(Program).Assembly;
        options.UseMultiTenancy = true;
        options.UseForwardedHeaders = true;
        options.UseAuthenticationPipeline = true;
        options.UseActiveUserValidation = true;
    });

    builder.Services.AddFgsBffApplication();
    builder.Services.AddFgsBffInfrastructure(builder.Configuration);
    await builder.LoadFgsRemoteCredentialsAsync();
    builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);

    builder.Services
        .AddGraphQLServer()
        .AddQueryType<BffQuery>();

    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UseFgsApiHost(hostOptions);
    app.MapFgsHealthChecks();
    app.MapGraphQL("/api/v1/bff/graphql").RequireAuthorization();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "FGS BFF service terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program;
