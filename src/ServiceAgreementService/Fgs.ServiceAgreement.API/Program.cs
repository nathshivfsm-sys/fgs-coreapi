using Fgs.ServiceAgreement.Application;

using Fgs.ServiceAgreement.Infrastructure;

using Fgs.Credentials.Extensions;
using Fgs.Foundation.Hosting;
using Fgs.Observability.Extensions;



var builder = WebApplication.CreateBuilder(args);



var hostOptions = builder.AddFgsApiHost(options =>

{

    options.ServiceName = "fgs-service-agreement-service";

    options.SwaggerTitle = "FGS Service Agreement Service";

    options.SwaggerDescription = "Service agreement management.";

    options.XmlCommentsAssembly = typeof(Program).Assembly;

});



builder.Services.AddFgsServiceAgreementApplication();

builder.Services.AddFgsServiceAgreementInfrastructure(builder.Configuration);

builder.Services.AddFgsObservability(builder.Configuration, hostOptions.ServiceName);



var app = builder.Build();
await app.LoadFgsRemoteCredentialsAsync();
app.UseFgsApiHost(hostOptions);

app.MapFgsHealthChecks();

app.Run();



public partial class Program;


