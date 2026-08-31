namespace Fgs.User.Application.Features.TenantMenus.Dtos;

public sealed record FgsTenantMenuDetailDto(
    long Id,
    int MenuId,
    short DisplayOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy);

public sealed record FgsTenantMenuSyncItemDto(
    int MenuId,
    short DisplayOrder = 1,
    bool IsActive = true);

/// <summary>
/// Sync payload: the tenant company's menu set becomes exactly <see cref="Items"/>.
/// Missing menu IDs are added, matching IDs are updated, DB rows not in the list are removed.
/// Empty list clears all menus for the tenant company.
/// </summary>
public sealed record FgsTenantMenuSyncDto(IReadOnlyList<FgsTenantMenuSyncItemDto> Items);
