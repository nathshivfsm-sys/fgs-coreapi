using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.ServiceAgreement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsServiceAgreementApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.ServiceAgreement");
        return services;
    }
}
