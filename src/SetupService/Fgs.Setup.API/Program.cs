using Fgs.Credentials;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;
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

builder.Services.AddFgsSetupApplication();
builder.Services.AddFgsSetupInfrastructure(builder.Configuration);
builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var loader = scope.ServiceProvider.GetRequiredService<CredentialConfigurationLoader>();
    await loader.ReloadAsync();
}

app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
