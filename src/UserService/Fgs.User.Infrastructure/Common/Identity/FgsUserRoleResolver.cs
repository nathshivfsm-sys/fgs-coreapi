using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;

namespace Fgs.User.Infrastructure.Common.Identity;

public sealed class FgsUserRoleResolver(IUserRoleCodesReadQuery roleCodesReadQuery) : IFgsUserRoleResolver
{
    public Task<IReadOnlyList<string>> ResolveRoleCodesAsync(
        Guid userId,
        CancellationToken cancellationToken = default) =>
        roleCodesReadQuery.GetRoleCodesForUserAsync(userId, cancellationToken);
}
