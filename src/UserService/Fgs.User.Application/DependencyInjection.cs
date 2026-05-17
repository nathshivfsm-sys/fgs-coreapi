using System.Reflection;
using Fgs.User.Application.Common.Behaviors;
using Fgs.User.Application.Signup;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsUserApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped<ISignupUniquenessValidator, SignupUniquenessValidator>();

        return services;
    }
}
