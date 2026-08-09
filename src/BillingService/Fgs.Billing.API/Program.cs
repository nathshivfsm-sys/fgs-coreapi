using Fgs.Billing.Application;
using Fgs.Billing.Infrastructure;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;

var builder = WebApplication.CreateBuilder(args);

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-billing-service";
    options.SwaggerTitle = "FGS Billing Service";
    options.SwaggerDescription = "Billing, invoicing, and payment terms.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsBillingApplication();
builder.Services.AddFgsBillingInfrastructure(builder.Configuration);
builder.AddFgsObservability(hostOptions.ServiceName);

var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
