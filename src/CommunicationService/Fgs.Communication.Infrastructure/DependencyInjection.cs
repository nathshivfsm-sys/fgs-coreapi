using Fgs.Credentials.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Communication.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsCommunicationInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-communication-service");
        return services;
    }
}
