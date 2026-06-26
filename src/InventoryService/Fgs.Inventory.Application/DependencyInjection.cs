using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Inventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsInventoryApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Inventory");
        return services;
    }
}
