using Fgs.Dispatch.Application;
using Fgs.Foundation.Api;
using Fgs.Foundation.Extensions;
using Fgs.MultiTenancy.Extensions;
using Fgs.Observability.Extensions;
using Fgs.Dispatch.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFgsApiVersioning();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureFgsApi());
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ConfigureFgsApi());
builder.Services.AddFgsSwagger(options =>
{
    options.Title = "FGS Dispatch Service";
    options.Description = "Field dispatch and scheduling.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
});
builder.Services.AddFgsDispatchApplication();
builder.Services.AddFgsDispatchInfrastructure(builder.Configuration);
builder.Services.AddFgsMultiTenancy();
builder.Services.AddFgsObservability(builder.Configuration, "fgs-dispatch-service");

var app = builder.Build();

app.UseFgsFoundationMiddleware();
if (ShouldUseHttpsRedirection(app.Configuration))
{
    app.UseHttpsRedirection();
}

app.UseFgsSwagger();

app.UseAuthentication();
app.UseFgsTenantResolution();
app.UseAuthorization();
app.MapControllers();
app.MapFgsHealthChecks();

app.Run();

static bool ShouldUseHttpsRedirection(IConfiguration configuration) =>
    !string.Equals(configuration["DOTNET_RUNNING_IN_CONTAINER"], "true", StringComparison.OrdinalIgnoreCase)
    && (configuration["ASPNETCORE_URLS"]?.Contains("https://", StringComparison.OrdinalIgnoreCase) == true
        || !string.IsNullOrWhiteSpace(configuration["ASPNETCORE_HTTPS_PORT"]));

public partial class Program;
