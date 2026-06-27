using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Crm.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsCrmApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Crm");
        return services;
    }
}
