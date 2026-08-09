using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Notification.Application;
using Fgs.Notification.Infrastructure;
using Fgs.Observability.Extensions;

var builder = WebApplication.CreateBuilder(args);

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-notification-service";
    options.SwaggerTitle = "FGS Notification Service";
    options.SwaggerDescription =
        "Shared platform capability: notifications (email/SMS/push) dispatch, templates, providers, and delivery history.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseAuthenticationPipeline = false;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsNotificationApplication();
builder.Services.AddFgsNotificationInfrastructure(builder.Configuration);
builder.AddFgsObservability(hostOptions.ServiceName);

var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
