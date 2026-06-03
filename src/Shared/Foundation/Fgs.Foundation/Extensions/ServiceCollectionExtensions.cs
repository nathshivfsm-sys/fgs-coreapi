using Fgs.Foundation.Behaviors;
using Fgs.Foundation.Correlation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Foundation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFgsFoundation(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICorrelationContext, HttpCorrelationContext>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}
