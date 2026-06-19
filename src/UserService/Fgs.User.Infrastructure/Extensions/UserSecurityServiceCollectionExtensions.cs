using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.User.Infrastructure.Extensions;

public static class UserSecurityServiceCollectionExtensions
{
    public static IServiceCollection AddFgsUserFacingSecurity(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddFgsEntraAuthentication(configuration);
}
