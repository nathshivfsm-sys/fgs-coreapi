using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Integration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsIntegrationApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Integration");
        return services;
    }
}
