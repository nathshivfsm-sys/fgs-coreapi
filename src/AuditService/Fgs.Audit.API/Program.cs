using Fgs.Audit.Application;
using Fgs.Audit.Infrastructure;
using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;

var builder = WebApplication.CreateBuilder(args);

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-audit-service";
    options.SwaggerTitle = "FGS Audit Service";
    options.SwaggerDescription = "Audit logging and compliance trails.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsAuditApplication();
builder.Services.AddFgsAuditInfrastructure(builder.Configuration);
builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);

var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
