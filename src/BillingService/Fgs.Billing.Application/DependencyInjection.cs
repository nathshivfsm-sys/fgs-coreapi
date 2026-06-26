using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Billing.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsBillingApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Billing");
        return services;
    }
}
