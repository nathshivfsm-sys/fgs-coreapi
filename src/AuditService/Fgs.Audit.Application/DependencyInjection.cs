using System.Reflection;
using Fgs.Foundation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Audit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsAuditApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), "Fgs.Audit");
        return services;
    }
}
