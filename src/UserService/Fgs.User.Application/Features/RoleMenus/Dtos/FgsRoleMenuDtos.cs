namespace Fgs.User.Application.Features.RoleMenus.Dtos;

public sealed record FgsRoleMenuDetailDto(
    long Id,
    long RoleId,
    int MenuId,
    short DisplayOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy);

public sealed record FgsRoleMenuSyncItemDto(
    int MenuId,
    short DisplayOrder = 1,
    bool IsActive = true);

/// <summary>
/// Sync payload: the role's menu set becomes exactly <see cref="Items"/>.
/// Missing menu IDs are added, matching IDs are updated, DB rows not in the list are removed.
/// Empty list clears all menus for the role.
/// </summary>
public sealed record FgsRoleMenuSyncDto(long RoleId, IReadOnlyList<FgsRoleMenuSyncItemDto> Items);
