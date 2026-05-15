using Fgs.User.API.Middleware;
using Fgs.User.API.Swagger;
using Fgs.User.Application;
using Fgs.User.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Clear();
});

builder.Services.AddControllers();
builder.Services.AddFgsUserSwagger();
builder.Services.AddFgsUserApplication();
builder.Services.AddFgsUserInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseForwardedHeaders();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

if (app.Configuration.IsSwaggerEnabled(app.Environment))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FGS User Service v1");
        options.DocumentTitle = "FGS User Service — API";
        options.DisplayRequestDuration();
    });
}

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

var instanceId = Environment.MachineName;
app.MapGet("/api/health", () => new
{
    Service = "User Service",
    Instance = instanceId,
    Time = DateTime.UtcNow
});

app.Run();

public partial class Program;
