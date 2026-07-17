namespace Fgs.User.Application.Features.Permissions.Dtos;

public sealed record FgsPermissionSummaryDto(
    long Id,
    string PermissionCode,
    string Module,
    string Resource,
    string Action,
    string Name,
    string? Description,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsPermissionDetailDto(
    long Id,
    string PermissionCode,
    string Module,
    string Resource,
    string Action,
    string Name,
    string? Description,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsPermissionLookupDto(
    long Id,
    string PermissionCode,
    string Module,
    string Resource,
    string Action,
    string Name,
    short DisplayOrder);

public sealed record FgsPermissionCreateDto(
    string PermissionCode,
    string Module,
    string Resource,
    string Action,
    string Name,
    string? Description,
    short DisplayOrder = 1);

public sealed record FgsPermissionUpdateDto(
    string PermissionCode,
    string Module,
    string Resource,
    string Action,
    string Name,
    string? Description,
    short DisplayOrder);

public sealed record FgsPermissionPatchDto(
    string? PermissionCode,
    string? Module,
    string? Resource,
    string? Action,
    string? Name,
    string? Description,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsPermissionListFilters(
    string? PermissionCode = null,
    string? Module = null,
    string? Resource = null,
    string? Action = null);
