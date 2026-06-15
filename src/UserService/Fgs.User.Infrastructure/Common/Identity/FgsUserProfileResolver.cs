using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Infrastructure.Common.Identity;

public sealed class FgsUserProfileResolver(
    IUnitOfWork unitOfWork,
    IFgsUserRoleResolver roleResolver) : IFgsUserProfileResolver
{
    public async Task<FgsUserProfile?> ResolveByEntraObjectIdAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Repository<FgsUser>()
            .FirstOrDefaultAsync(
                u => u.EntraObjectId == entraObjectId && u.IsActive && !u.IsDeleted,
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
        var user = await unitOfWork.Repository<FgsUser>()
            .FirstOrDefaultAsync(
                u => u.Email == normalizedEmail && u.IsActive && !u.IsDeleted,
                cancellationToken);

        if (user is null)
        {
            return null;
        }

        var hasValidInvitation = await unitOfWork.Repository<FgsInvitation>()
            .AnyAsync(
                i => i.UserId == user.Id
                     && (i.Status == InvitationStatus.Pending || i.Status == InvitationStatus.Accepted),
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
        var roles = await roleResolver.ResolveRoleCodesAsync(user.Id, cancellationToken);

        return new FgsUserProfile(
            user.Id,
            user.Email,
            user.EntraObjectId,
            user.TenantId,
            user.CompanyId,
            roles);
    }
}
