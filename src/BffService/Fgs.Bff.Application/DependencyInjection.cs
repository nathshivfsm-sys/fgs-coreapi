using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Bff.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsBffApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Bff");
        return services;
    }
}
