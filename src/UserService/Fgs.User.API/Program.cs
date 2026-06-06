using Fgs.Foundation.Api;
using Fgs.Foundation.Extensions;
using Fgs.MultiTenancy.Extensions;
using Fgs.Observability.Extensions;
using Fgs.Foundation.Middleware;
using Fgs.User.Application;
using Fgs.User.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

var kmsKeyArnFromEnv = Environment.GetEnvironmentVariable("KMS_KEY_ARN");
if (!string.IsNullOrWhiteSpace(kmsKeyArnFromEnv))
{
    builder.Configuration["AwsCredentials:KmsKeyArn"] = kmsKeyArnFromEnv;
}

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Clear();
});

builder.Services.AddFgsApiVersioning();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureFgsApi());
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ConfigureFgsApi());
builder.Services.AddFgsSwagger(options =>
{
    options.Title = "FGS User Service";
    options.Description =
        "Multi-tenant company onboarding (signup), email invitations, Microsoft Entra External ID callback, "
        + "transactional outbox, and platform user management.";
    options.ContactName = "FGS Platform";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
});
builder.Services.AddFgsUserApplication();
builder.Services.AddFgsUserInfrastructure(builder.Configuration);
builder.Services.AddFgsMultiTenancy();
builder.Services.AddFgsObservability(builder.Configuration, "fgs-user-service");

var app = builder.Build();

app.UseForwardedHeaders();
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
