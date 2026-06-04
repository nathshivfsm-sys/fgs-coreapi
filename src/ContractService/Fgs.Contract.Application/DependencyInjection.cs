using System.Reflection;
using Fgs.Foundation.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Contract.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsContractApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddFgsFoundation();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}
