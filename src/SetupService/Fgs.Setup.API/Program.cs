using Fgs.Credentials;
using Fgs.Foundation.Caching.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;
using Fgs.Setup.API.Swagger;
using Fgs.Setup.Application;
using Fgs.Setup.Infrastructure;
using Fgs.Setup.Infrastructure.Credentials;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.ApplyFgsAwsCredentialEnvironmentVariables();

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-setup-service";
    options.SwaggerTitle = "FGS Setup Service";
    options.SwaggerDescription = "Tenant and system setup configuration.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsSetupSwagger();
builder.Services.AddFgsSetupApplication();
builder.Services.AddFgsSetupInfrastructure(builder.Configuration);
await builder.LoadFgsSetupCredentialsAsync();
builder.Services.AddFgsRedisCache(builder.Configuration);
builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);

var app = builder.Build();

app.UseFgsApiHost(hostOptions);

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

app.MapFgsHealthChecks();
app.Run();

public partial class Program;
