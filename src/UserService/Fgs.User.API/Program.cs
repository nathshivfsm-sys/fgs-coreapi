using Fgs.Credentials;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Caching.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;
using Fgs.User.Application;
using Fgs.User.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.ApplyFgsKmsEnvironmentVariable();

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-user-service";
    options.SwaggerTitle = "FGS User Service";
    options.SwaggerDescription =
        "Multi-tenant company onboarding (signup), email invitations, Microsoft Entra External ID callback, "
        + "transactional outbox, and platform user management.";
    options.SwaggerContactName = "FGS Platform";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
    options.UseForwardedHeaders = true;
});

builder.Services.AddFgsUserApplication();
builder.Services.AddFgsUserInfrastructure(builder.Configuration);
await builder.LoadFgsRemoteCredentialsAsync();
builder.Services.AddFgsRedisCache(builder.Configuration);
builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);

var app = builder.Build();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
