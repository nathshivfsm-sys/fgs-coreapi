using Fgs.Integration.Application;

using Fgs.Integration.Infrastructure;

using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;



var builder = WebApplication.CreateBuilder(args);



var hostOptions = builder.AddFgsApiHost(options =>

{

    options.ServiceName = "fgs-integration-service";

    options.SwaggerTitle = "FGS Integration Service";

    options.SwaggerDescription = "External integrations.";

    options.XmlCommentsAssembly = typeof(Program).Assembly;

});



builder.Services.AddFgsIntegrationApplication();

builder.Services.AddFgsIntegrationInfrastructure(builder.Configuration);

builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);



var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);

app.MapFgsHealthChecks();

app.Run();



public partial class Program;


