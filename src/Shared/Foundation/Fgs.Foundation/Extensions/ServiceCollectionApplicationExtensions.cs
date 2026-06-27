using System.Reflection;
using Fgs.Contracts.Api;
using Fgs.Contracts.Health;
using Fgs.Foundation.Health;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Foundation.Extensions;

public static class ServiceCollectionApplicationExtensions
{
    public static IServiceCollection AddFgsApplicationLayer(
        this IServiceCollection services,
        Assembly applicationAssembly,
        string? serviceHealthName = null)
    {
        services.AddFgsFoundation();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        if (serviceHealthName is not null)
        {
            services.AddTransient<IRequestHandler<GetServiceHealthQuery, ApiResponse<ServiceHealthDto>>>(
                _ => new GetServiceHealthQueryHandler(serviceHealthName));
        }

        services.AddValidatorsFromAssembly(applicationAssembly);
        return services;
    }
}
