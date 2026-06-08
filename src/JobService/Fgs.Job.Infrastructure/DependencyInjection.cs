using Fgs.Credentials.Extensions;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Job.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsJobInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-job-service";
                options.RequiredProviders = ["DATABASE"];
            });

        services.AddFgsApiSecurity(configuration);
        return services;
    }
}
