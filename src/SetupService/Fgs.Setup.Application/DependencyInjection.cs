using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Setup.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsSetupApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Setup");
        services.AddScoped<Features.Credentials.Services.CredentialMutationService>();
        return services;
    }
}
