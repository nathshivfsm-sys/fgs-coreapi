using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;
using Fgs.Publisher.Application;
using Fgs.Publisher.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-publisher-service";
    options.SwaggerTitle = "FGS Publisher Service";
    options.SwaggerDescription = "Message publishing to the event bus.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsPublisherApplication();
builder.Services.AddFgsPublisherInfrastructure(builder.Configuration);
builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);

var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
