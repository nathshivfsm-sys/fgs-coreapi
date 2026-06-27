using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Communication.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsCommunicationApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Communication");
        return services;
    }
}
