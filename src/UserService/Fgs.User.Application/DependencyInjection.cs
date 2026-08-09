using System.Reflection;
using Fgs.Foundation.Extensions;
using Fgs.User.Application.Abstractions.Invitations;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Invitations;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsUserApplication(this IServiceCollection services)
    {
        services.AddFgsApplicationLayer(Assembly.GetExecutingAssembly(), null);
        services.AddScoped<ISignupUniquenessValidator, SignupUniquenessValidator>();
        services.AddScoped<IUserInvitationIssuer, UserInvitationIssuer>();
        return services;
    }
}
