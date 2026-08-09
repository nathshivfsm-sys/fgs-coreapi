using Fgs.Inventory.Application;
using Fgs.Inventory.Infrastructure;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Caching.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;

var builder = WebApplication.CreateBuilder(args);

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-inventory-service";
    options.SwaggerTitle = "FGS Inventory Service";
    options.SwaggerDescription = "Inventory management.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsInventoryApplication();
builder.Services.AddFgsInventoryInfrastructure(builder.Configuration);
await builder.LoadFgsRemoteCredentialsAsync();
builder.Services.AddFgsRedisCache(builder.Configuration);
builder.AddFgsObservability(hostOptions.ServiceName);

var app = builder.Build();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
