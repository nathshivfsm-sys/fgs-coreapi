using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.WorkOrder.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsWorkOrderInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FgsWorkOrder");
        return services;
    }
}
