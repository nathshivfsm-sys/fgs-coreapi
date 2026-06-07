using Fgs.Foundation.Api;
using Fgs.Foundation.Extensions;
using Fgs.MultiTenancy.Extensions;
using Fgs.Observability.Extensions;
using Fgs.Setup.Application;
using Fgs.Setup.Infrastructure;
using Fgs.Setup.Infrastructure.Credentials;

var builder = WebApplication.CreateBuilder(args);

var kmsKeyArnFromEnv = Environment.GetEnvironmentVariable("KMS_KEY_ARN");
if (!string.IsNullOrWhiteSpace(kmsKeyArnFromEnv))
{
    builder.Configuration["AwsCredentials:KmsKeyArn"] = kmsKeyArnFromEnv;
}

var awsAccessKeyFromEnv = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
if (!string.IsNullOrWhiteSpace(awsAccessKeyFromEnv))
{
    builder.Configuration["AwsCredentials:AccessKeyId"] = awsAccessKeyFromEnv;
}

var awsSecretKeyFromEnv = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
if (!string.IsNullOrWhiteSpace(awsSecretKeyFromEnv))
{
    builder.Configuration["AwsCredentials:SecretAccessKey"] = awsSecretKeyFromEnv;
}

var credentialDistributionKeyFromEnv = Environment.GetEnvironmentVariable("CREDENTIAL_DISTRIBUTION_KEY");
if (!string.IsNullOrWhiteSpace(credentialDistributionKeyFromEnv))
{
    builder.Configuration["CredentialDistribution:InternalServiceKey"] = credentialDistributionKeyFromEnv;
}

builder.Services.AddFgsApiVersioning();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureFgsApi());
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ConfigureFgsApi());
builder.Services.AddFgsSwagger(options =>
{
    options.Title = "FGS Setup Service";
    options.Description = "Tenant and system setup configuration.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
});
builder.Services.AddFgsSetupApplication();
builder.Services.AddFgsSetupInfrastructure(builder.Configuration);
builder.Services.AddFgsMultiTenancy();
builder.Services.AddFgsObservability(builder.Configuration, "fgs-setup-service");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var loader = scope.ServiceProvider.GetRequiredService<CredentialConfigurationLoader>();
    await loader.ReloadAsync();
}

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
