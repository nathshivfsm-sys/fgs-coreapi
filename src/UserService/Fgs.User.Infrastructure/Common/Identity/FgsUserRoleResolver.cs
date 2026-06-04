using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Infrastructure.Common.Identity;

public sealed class FgsUserRoleResolver(IUnitOfWork unitOfWork) : IFgsUserRoleResolver
{
    public async Task<IReadOnlyList<string>> ResolveRoleCodesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userRoles = await unitOfWork.Repository<FgsUserRole>()
            .ListAsync(ur => ur.UserId == userId, cancellationToken);

        if (userRoles.Count == 0)
        {
            return [];
        }

        var fgsRoleRepo = unitOfWork.Repository<FgsRole>();
        var roleCodes = new List<string>(userRoles.Count);

        foreach (var userRole in userRoles)
        {
            if (userRole.FgsRoleId is { } fgsRoleId)
            {
                var fgsRole = await fgsRoleRepo.FirstOrDefaultAsync(r => r.Id == fgsRoleId, cancellationToken);
                if (fgsRole is not null)
                {
                    roleCodes.Add(fgsRole.RoleCode);
                }
            }
        }

        return roleCodes;
    }
}
