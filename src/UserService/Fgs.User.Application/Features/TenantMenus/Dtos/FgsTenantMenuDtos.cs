namespace Fgs.User.Application.Features.TenantMenus.Dtos;

public sealed record FgsTenantMenuDetailDto(
    long Id,
    int MenuId,
    string MenuCode,
    string Name,
    string? Description,
    int? ParentMenuId,
    string MenuType,
    string? Route,
    string? Icon,
    short DisplayOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy);

public sealed record FgsTenantMenuLookupDto(
    long Id,
    int MenuId,
    string MenuCode,
    string Name,
    short DisplayOrder);

public sealed record FgsTenantMenuCreateDto(
    int MenuId,
    string MenuCode,
    string Name,
    string MenuType,
    string? Description = null,
    int? ParentMenuId = null,
    string? Route = null,
    string? Icon = null,
    short DisplayOrder = 1);

public sealed record FgsTenantMenuUpdateDto(
    int MenuId,
    string MenuCode,
    string Name,
    string MenuType,
    string? Description = null,
    int? ParentMenuId = null,
    string? Route = null,
    string? Icon = null,
    short DisplayOrder = 1);

public sealed record FgsTenantMenuPatchDto(
    int? MenuId = null,
    string? MenuCode = null,
    string? Name = null,
    string? Description = null,
    int? ParentMenuId = null,
    string? MenuType = null,
    string? Route = null,
    string? Icon = null,
    short? DisplayOrder = null,
    bool? IsActive = null);

public sealed record FgsTenantMenuSyncItemDto(
    int MenuId,
    string MenuCode,
    string Name,
    string MenuType,
    string? Description = null,
    int? ParentMenuId = null,
    string? Route = null,
    string? Icon = null,
    short DisplayOrder = 1,
    bool IsActive = true);

/// <summary>
/// Sync payload: the tenant company's menu set becomes exactly <see cref="Items"/>.
/// Missing menu IDs are added, matching IDs are updated, DB rows not in the list are removed.
/// Empty list clears all menus for the tenant company.
/// </summary>
public sealed record FgsTenantMenuSyncDto(IReadOnlyList<FgsTenantMenuSyncItemDto> Items);
