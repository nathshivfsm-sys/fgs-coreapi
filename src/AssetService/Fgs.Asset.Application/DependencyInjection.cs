using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Asset.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsAssetApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Asset");
        return services;
    }
}
