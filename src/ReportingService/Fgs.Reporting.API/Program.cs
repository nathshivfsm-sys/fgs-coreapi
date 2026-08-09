using Fgs.Reporting.Application;

using Fgs.Reporting.Infrastructure;

using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;



var builder = WebApplication.CreateBuilder(args);



var hostOptions = builder.AddFgsApiHost(options =>

{

    options.ServiceName = "fgs-reporting-service";

    options.SwaggerTitle = "FGS Reporting Service";

    options.SwaggerDescription = "Reporting and analytics.";

    options.XmlCommentsAssembly = typeof(Program).Assembly;

});



builder.Services.AddFgsReportingApplication();

builder.Services.AddFgsReportingInfrastructure(builder.Configuration);

builder.AddFgsObservability(hostOptions.ServiceName);



var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);

app.MapFgsHealthChecks();

app.Run();



public partial class Program;


