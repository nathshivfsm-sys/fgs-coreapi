using Fgs.Job.Application;

using Fgs.Job.Infrastructure;

using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;



var builder = WebApplication.CreateBuilder(args);



var hostOptions = builder.AddFgsApiHost(options =>

{

    options.ServiceName = "fgs-job-service";

    options.SwaggerTitle = "FGS Job Service";

    options.SwaggerDescription = "Job management.";

    options.XmlCommentsAssembly = typeof(Program).Assembly;

});



builder.Services.AddFgsJobApplication();

builder.Services.AddFgsJobInfrastructure(builder.Configuration);

builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);



var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);

app.MapFgsHealthChecks();

app.Run();



public partial class Program;


