using System.Reflection;
using Fgs.Foundation.Extensions;
using Fgs.User.Application.Features.Signup;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsUserApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), null);
        services.AddScoped<ISignupUniquenessValidator, SignupUniquenessValidator>();
        return services;
    }
}
