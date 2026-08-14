namespace Fgs.User.Application.Features.UserRoles.Dtos;

public sealed record FgsUserRoleSummaryDto(
    long Id,
    Guid UserId,
    long FgsRoleId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsUserRoleDetailDto(
    long Id,
    Guid UserId,
    long FgsRoleId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

/// <summary>
/// Sync payload: the user's role set becomes exactly <see cref="FgsRoleIds"/>.
/// Missing IDs are added, matching IDs are kept, DB rows not in the list are removed.
/// Empty list clears all roles for the user.
/// </summary>
public sealed record FgsUserRoleSyncDto(Guid UserId, IReadOnlyList<long> FgsRoleIds);

