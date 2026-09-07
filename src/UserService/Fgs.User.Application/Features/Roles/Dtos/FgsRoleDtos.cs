namespace Fgs.User.Application.Features.Roles.Dtos;

public sealed record FgsRoleSummaryDto(
    long Id,
    string RoleCode,
    string Name,
    string? Description,
    long? ParentRoleId,
    bool IsBuiltIn,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsRoleDetailDto(
    long Id,
    string RoleCode,
    string Name,
    string? Description,
    long? ParentRoleId,
    bool IsBuiltIn,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsRoleLookupDto(
    long Id,
    string RoleCode,
    string Name,
    bool IsBuiltIn,
    short DisplayOrder);

public sealed record FgsRoleCreateDto(
    string RoleCode,
    string Name,
    string? Description,
    long? ParentRoleId = null,
    short DisplayOrder = 1);

/// <summary>
/// Payload for cloning an existing role. When <see cref="FgsPermissionIds"/> is null,
/// all permissions from the source role are copied; otherwise the provided set is used.
/// </summary>
public sealed record FgsRoleCloneDto(
    string RoleCode,
    string Name,
    string? Description = null,
    short? DisplayOrder = null,
    IReadOnlyList<long>? FgsPermissionIds = null);

public sealed record FgsRoleUpdateDto(
    string RoleCode,
    string Name,
    string? Description,
    short DisplayOrder);

public sealed record FgsRolePatchDto(
    string? RoleCode,
    string? Name,
    string? Description,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsRoleListFilters(
    string? RoleCode = null,
    string? Name = null,
    bool? IsBuiltIn = null);
