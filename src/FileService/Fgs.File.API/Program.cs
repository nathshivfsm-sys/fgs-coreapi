using Fgs.Credentials;
using Fgs.Credentials.Extensions;
using Fgs.File.Application;
using Fgs.File.Infrastructure;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.ApplyFgsKmsEnvironmentVariable();

var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-file-service";
    options.SwaggerTitle = "FGS File Service";
    options.SwaggerDescription = "File storage and document management.";
    options.XmlCommentsAssembly = typeof(Program).Assembly;
    options.UseMultiTenancy = true;
});

builder.Services.AddFgsFileApplication();
builder.Services.AddFgsFileInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<Fgs.File.Application.Abstractions.Storage.IAttachmentUrlBuilder, Fgs.File.API.Services.AttachmentUrlBuilder>();
builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);

var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
app.Run();

public partial class Program;
