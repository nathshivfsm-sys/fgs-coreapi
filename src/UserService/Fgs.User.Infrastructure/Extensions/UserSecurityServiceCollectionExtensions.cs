using Fgs.Security.Extensions;
using Fgs.Security.UserAuth;
using Fgs.User.Infrastructure.Common.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fgs.User.Infrastructure.Extensions;

public static class UserSecurityServiceCollectionExtensions
{
    public static IServiceCollection AddFgsUserFacingSecurity(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddFgsEntraAuthentication(configuration)
            .AddFgsActiveUserValidation(configuration)
            .AddScoped<IUserAuthProfileSource, UserServiceAuthProfileSource>();
}
