using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Notification.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsNotificationApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Notification");
        return services;
    }
}
