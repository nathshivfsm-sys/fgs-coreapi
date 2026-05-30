using System.Reflection;
using Fgs.Foundation.Extensions;
using Fgs.User.Application.Features.Signup;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsUserApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddFgsFoundation();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped<ISignupUniquenessValidator, SignupUniquenessValidator>();

        return services;
    }
}
