using Fgs.Contracts.Auth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Infrastructure.Common.Identity;

public sealed class LoginAuthorizationProfileBuilder(
    IUserRoleCodesReadQuery roleCodesReadQuery,
    IUserAuthorizationReadQuery authorizationReadQuery,
    IFgsPublicEndpointReadRepository publicEndpointReadRepository) : ILoginAuthorizationProfileBuilder
{
    public async Task<FgsUserProfile> BuildAsync(FgsUser user, CancellationToken cancellationToken = default)
    {
        var roles = await roleCodesReadQuery.GetRoleCodesForUserAsync(user.Id, cancellationToken);
        var permissions = await authorizationReadQuery.GetPermissionCodesForUserAsync(user.Id, cancellationToken);
        var dataAccess = await authorizationReadQuery.GetDataAccessCodesForUserAsync(user.Id, cancellationToken);
        var endpoints = await publicEndpointReadRepository.ListActiveForTenantCompanyAsync(
            user.TenantId,
            user.CompanyId,
            cancellationToken);

        return new FgsUserProfile(
            user.Id,
            user.Email,
            user.EntraObjectId,
            user.TenantId,
            user.CompanyId,
            user.IsActive,
            user.IsDeleted,
            roles,
            permissions,
            dataAccess,
            endpoints.Select(e => new PublicEndpointAuthDto(
                e.EndpointType,
                e.EnvironmentCode,
                e.BaseUrl,
                e.DisplayName)).ToList());
    }
}
