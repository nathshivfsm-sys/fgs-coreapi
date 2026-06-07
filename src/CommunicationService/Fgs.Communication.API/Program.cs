using Fgs.Communication.Application;

using Fgs.Communication.Infrastructure;

using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;



var builder = WebApplication.CreateBuilder(args);



var hostOptions = builder.AddFgsApiHost(options =>

{

    options.ServiceName = "fgs-communication-service";

    options.SwaggerTitle = "FGS Communication Service";

    options.SwaggerDescription = "Communication and messaging.";

    options.XmlCommentsAssembly = typeof(Program).Assembly;

});



builder.Services.AddFgsCommunicationApplication();

builder.Services.AddFgsCommunicationInfrastructure(builder.Configuration);

builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);



var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);

app.MapFgsHealthChecks();

app.Run();



public partial class Program;


