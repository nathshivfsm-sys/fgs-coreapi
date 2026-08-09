using Fgs.Asset.Application;
using Fgs.Asset.Infrastructure;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Caching.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;

var builder = WebApplication.CreateBuilder(args);

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-asset-service";
    options.SwaggerTitle = "FGS Asset Service";
    options.SwaggerDescription = "Asset management.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsAssetApplication();
builder.Services.AddFgsAssetInfrastructure(builder.Configuration);
await builder.LoadFgsRemoteCredentialsAsync();
builder.Services.AddFgsRedisCache(builder.Configuration);
builder.AddFgsObservability(hostOptions.ServiceName);

var app = builder.Build();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
