using Fgs.Crm.Application;
using Fgs.Crm.Infrastructure;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;

var builder = WebApplication.CreateBuilder(args);

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-crm-service";
    options.SwaggerTitle = "FGS CRM Service";
    options.SwaggerDescription = "Customer relationship management.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsCrmApplication();
builder.Services.AddFgsCrmInfrastructure(builder.Configuration);
builder.AddFgsObservability(hostOptions.ServiceName);

var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
