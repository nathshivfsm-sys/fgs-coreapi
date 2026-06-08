using Fgs.Dispatch.Application;

using Fgs.Dispatch.Infrastructure;

using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;



var builder = WebApplication.CreateBuilder(args);



var hostOptions = builder.AddFgsApiHost(options =>

{

    options.ServiceName = "fgs-dispatch-service";

    options.SwaggerTitle = "FGS Dispatch Service";

    options.SwaggerDescription = "Dispatch and scheduling.";

    options.XmlCommentsAssembly = typeof(Program).Assembly;

});



builder.Services.AddFgsDispatchApplication();

builder.Services.AddFgsDispatchInfrastructure(builder.Configuration);

builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);



var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);

app.MapFgsHealthChecks();

app.Run();



public partial class Program;


