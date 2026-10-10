using Fgs.Security.UserAuth;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Common.Auth;

/// <summary>
/// Drops cached auth profiles after role, permission, or data-access changes.
/// </summary>
public sealed class UserAuthProfileInvalidator(
    FgsUserDbContext context,
    IUserAuthProfileStore profileStore)
{
    public async Task InvalidateUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var entraObjectId = await context.FgsUsers
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => user.EntraObjectId)
            .FirstOrDefaultAsync(cancellationToken);

        await profileStore.InvalidateAsync(userId, entraObjectId, cancellationToken);
    }

    public async Task InvalidateUsersAssignedToRoleAsync(
        long roleId,
        CancellationToken cancellationToken = default)
    {
        var userIds = await context.FgsUserRoles
            .AsNoTracking()
            .Where(assignment => assignment.FgsRoleId == roleId)
            .Select(assignment => assignment.UserId)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (userIds.Count == 0)
        {
            return;
        }

        var entraByUserId = await context.FgsUsers
            .AsNoTracking()
            .Where(user => userIds.Contains(user.Id))
            .Select(user => new { user.Id, user.EntraObjectId })
            .ToDictionaryAsync(user => user.Id, user => user.EntraObjectId, cancellationToken);

        foreach (var userId in userIds)
        {
            entraByUserId.TryGetValue(userId, out var entraObjectId);
            await profileStore.InvalidateAsync(userId, entraObjectId, cancellationToken);
        }
    }
}
