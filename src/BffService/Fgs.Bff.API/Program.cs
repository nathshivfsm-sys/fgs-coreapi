using Fgs.Bff.API.GraphQL;
using Fgs.Bff.Application;
using Fgs.Bff.Infrastructure;
using Fgs.Credentials;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;
using Fgs.Observability.Logging;
using Serilog;

// Non-reloadable logger: LoadFgsRemoteCredentialsAsync builds a temporary ServiceProvider
// before WebApplication.Build(), which freezes Serilog's CreateBootstrapLogger().
Log.Logger = SerilogHostExtensions.CreateFgsBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.ApplyFgsKmsEnvironmentVariable();

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
    builder.AddFgsObservability(hostOptions.ServiceName);

    builder.Services
        .AddGraphQLServer()
        .AddQueryType<BffQuery>()
        .AddDiagnosticEventListener<Fgs.Bff.API.GraphQL.FgsGraphQlDiagnosticObserver>();

    var app = builder.Build();
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
