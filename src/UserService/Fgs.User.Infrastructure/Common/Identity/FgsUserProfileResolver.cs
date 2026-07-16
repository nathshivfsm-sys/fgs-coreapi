using Fgs.Contracts.Auth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Infrastructure.Common.Identity;

public sealed class FgsUserProfileResolver(
    IUserReadRepository<FgsUser> userReadRepository,
    IInvitationReadQuery invitationReadQuery,
    IUserRoleCodesReadQuery roleCodesReadQuery,
    IUserAuthorizationReadQuery authorizationReadQuery,
    IFgsPublicEndpointReadRepository publicEndpointReadRepository) : IFgsUserProfileResolver
{
    public async Task<FgsUserProfile?> ResolveByEntraObjectIdAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default)
    {
        var user = await userReadRepository.FirstOrDefaultAsync(
            "\"EntraObjectId\" = @entraObjectId AND \"IsActive\" = true AND \"IsDeleted\" = false",
            new { entraObjectId },
            cancellationToken);

        if (user is null || string.IsNullOrWhiteSpace(user.EntraObjectId))
        {
            return null;
        }

        return await ToProfileAsync(user, cancellationToken);
    }

    public async Task<FgsUserProfile?> ResolveBySignupEmailAsync(
        string normalizedEmail,
        CancellationToken cancellationToken = default)
    {
        var user = await userReadRepository.FirstOrDefaultAsync(
            "\"Email\" = @email AND \"IsActive\" = true AND \"IsDeleted\" = false",
            new { email = normalizedEmail },
            cancellationToken);

        if (user is null)
        {
            return null;
        }

        var hasValidInvitation = await invitationReadQuery.HasValidInvitationForUserAsync(
            user.Id,
            cancellationToken);

        if (!hasValidInvitation)
        {
            return null;
        }

        return await ToProfileAsync(user, cancellationToken);
    }

    public async Task<FgsUserProfile?> ResolveForEntraConnectorAsync(
        string? objectId,
        string? email,
        CancellationToken cancellationToken = default)
    {
        FgsUserProfile? profile = null;

        if (!string.IsNullOrWhiteSpace(objectId))
        {
            profile = await ResolveByEntraObjectIdAsync(objectId, cancellationToken);
        }

        if (profile is null && !string.IsNullOrWhiteSpace(email))
        {
            profile = await ResolveBySignupEmailAsync(email, cancellationToken);
        }

        if (profile is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(objectId)
            && !string.IsNullOrWhiteSpace(profile.EntraObjectId)
            && !string.Equals(profile.EntraObjectId, objectId, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return profile;
    }

    private async Task<FgsUserProfile> ToProfileAsync(FgsUser user, CancellationToken cancellationToken)
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
