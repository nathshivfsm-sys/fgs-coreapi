using Fgs.Credentials.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Billing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsBillingInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-billing-service", "DATABASE");
        return services;
    }
}
