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

public sealed record FgsRolePermissionCreateDto(long FgsRoleId, long FgsPermissionId);

public sealed record FgsRolePermissionListFilters(long? FgsRoleId = null, long? FgsPermissionId = null);
