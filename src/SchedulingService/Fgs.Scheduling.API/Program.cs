using Fgs.Scheduling.Application;
using Fgs.Scheduling.Infrastructure;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;

var builder = WebApplication.CreateBuilder(args);

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-scheduling-service";
    options.SwaggerTitle = "FGS Scheduling Service";
    options.SwaggerDescription = "Scheduling and dispatch.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsSchedulingApplication();
builder.Services.AddFgsSchedulingInfrastructure(builder.Configuration);
builder.AddFgsObservability(hostOptions.ServiceName);

var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
