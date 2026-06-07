using Fgs.Contract.Application;

using Fgs.Contract.Infrastructure;

using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;



var builder = WebApplication.CreateBuilder(args);



var hostOptions = builder.AddFgsApiHost(options =>

{

    options.ServiceName = "fgs-contract-service";

    options.SwaggerTitle = "FGS Contract Service";

    options.SwaggerDescription = "Contract management.";

    options.XmlCommentsAssembly = typeof(Program).Assembly;

});



builder.Services.AddFgsContractApplication();

builder.Services.AddFgsContractInfrastructure(builder.Configuration);

builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);



var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);

app.MapFgsHealthChecks();

app.Run();



public partial class Program;


