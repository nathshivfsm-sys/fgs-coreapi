namespace Fgs.User.Application.Features.RoleDataAccesses.Dtos;

public sealed record FgsRoleDataAccessSummaryDto(
    long Id,
    long FgsRoleId,
    long FgsDataAccessId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsRoleDataAccessDetailDto(
    long Id,
    long FgsRoleId,
    long FgsDataAccessId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

/// <summary>
/// Sync payload: the role's data-access set becomes exactly <see cref="FgsDataAccessIds"/>.
/// Missing IDs are added, matching IDs are kept, DB rows not in the list are removed.
/// Empty list clears all data-access assignments for the role.
/// </summary>
public sealed record FgsRoleDataAccessSyncDto(long FgsRoleId, IReadOnlyList<long> FgsDataAccessIds);

