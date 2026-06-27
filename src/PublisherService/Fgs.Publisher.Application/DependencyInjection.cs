using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Publisher.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsPublisherApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Publisher");
        return services;
    }
}
