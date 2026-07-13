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
