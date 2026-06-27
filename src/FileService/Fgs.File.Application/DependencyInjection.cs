using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.File.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsFileApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.File");
        return services;
    }
}
