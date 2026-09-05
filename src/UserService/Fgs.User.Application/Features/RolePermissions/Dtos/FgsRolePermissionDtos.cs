namespace Fgs.User.Application.Features.RolePermissions.Dtos;

public sealed record FgsRolePermissionSummaryDto(
    long Id,
    long FgsRoleId,
    long FgsPermissionId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsRolePermissionDetailDto(
    long Id,
    long FgsRoleId,
    long FgsPermissionId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsRolePermissionLookupDto(
    long Id,
    long FgsRoleId,
    long FgsPermissionId);

public sealed record FgsRolePermissionCreateDto(
    long FgsRoleId,
    long FgsPermissionId);

public sealed record FgsRolePermissionUpdateDto(
    long FgsPermissionId);

public sealed record FgsRolePermissionPatchDto(
    long? FgsPermissionId = null);

/// <summary>
/// Sync payload: the role's permission set becomes exactly <see cref="FgsPermissionIds"/>.
/// Missing IDs are added, matching IDs are kept, DB rows not in the list are removed.
/// Empty list clears all permissions for the role.
/// </summary>
public sealed record FgsRolePermissionSyncDto(long FgsRoleId, IReadOnlyList<long> FgsPermissionIds);
