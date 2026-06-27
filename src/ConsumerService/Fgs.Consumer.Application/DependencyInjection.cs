using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Consumer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsConsumerApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Consumer");
        return services;
    }
}
