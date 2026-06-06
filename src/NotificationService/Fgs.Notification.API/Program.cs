using Fgs.Foundation.Api;
using Fgs.Foundation.Extensions;
using Fgs.MultiTenancy.Extensions;
using Fgs.Observability.Extensions;
using Fgs.Notification.Application;
using Fgs.Notification.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFgsApiVersioning();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureFgsApi());
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ConfigureFgsApi());
builder.Services.AddFgsSwagger(options =>
{
    options.Title = "FGS Notification Service";
    options.Description =
        "Shared platform capabilities: notifications (email/SMS/push), integrations, audit, background jobs, and reporting foundations.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
});
builder.Services.AddFgsNotificationApplication();
builder.Services.AddFgsNotificationInfrastructure(builder.Configuration);
builder.Services.AddFgsMultiTenancy();
builder.Services.AddFgsObservability(builder.Configuration, "fgs-notification-service");

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
