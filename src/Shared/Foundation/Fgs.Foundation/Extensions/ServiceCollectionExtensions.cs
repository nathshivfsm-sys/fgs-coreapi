using Fgs.Foundation.Behaviors;
using Fgs.Foundation.Correlation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fgs.Foundation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFgsFoundation(this IServiceCollection services)
    {
        services.Configure<LoggerFilterOptions>(options =>
            options.Rules.Add(new LoggerFilterRule(
                null,
                "Microsoft.EntityFrameworkCore.Database.Command",
                LogLevel.Warning,
                null)));

        services.AddHttpContextAccessor();
        services.AddScoped<ICorrelationContext, HttpCorrelationContext>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}
