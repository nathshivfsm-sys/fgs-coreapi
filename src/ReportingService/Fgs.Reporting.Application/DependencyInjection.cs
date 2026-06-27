using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Reporting.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsReportingApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Reporting");
        return services;
    }
}
