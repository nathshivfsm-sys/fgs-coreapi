using Fgs.Consumer.Application;
using Fgs.Consumer.Infrastructure;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;

var builder = WebApplication.CreateBuilder(args);

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-consumer-service";
    options.SwaggerTitle = "FGS Consumer Service";
    options.SwaggerDescription = "Integration event consumers.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
    options.UseAuthenticationPipeline = false;
});

builder.Services.AddFgsConsumerApplication();
builder.Services.AddFgsConsumerInfrastructure(builder.Configuration);
builder.AddFgsObservability(hostOptions.ServiceName);

var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
