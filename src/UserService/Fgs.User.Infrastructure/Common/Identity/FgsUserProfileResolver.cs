using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Domain.Entities;

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
            .FirstOrDefaultAsync(u => u.EntraObjectId == entraObjectId && u.IsActive && !u.IsDeleted, cancellationToken);

        if (user is null || string.IsNullOrWhiteSpace(user.EntraObjectId))
        {
            return null;
        }

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
