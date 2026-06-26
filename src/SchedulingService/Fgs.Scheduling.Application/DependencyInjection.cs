using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Scheduling.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsSchedulingApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Scheduling");
        return services;
    }
}
