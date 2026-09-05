using Fgs.User.Application.Features.TenantMenus.Dtos;

namespace Fgs.User.Infrastructure.Entities.TenantMenus;

internal sealed class FgsTenantMenuRow
{
    public long Id { get; set; }
    public int MenuId { get; set; }
    public string MenuCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? ParentMenuId { get; set; }
    public string MenuType { get; set; } = null!;
    public string? Route { get; set; }
    public string? Icon { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }

    public FgsTenantMenuDetailDto ToDetailDto() =>
        new(
            Id,
            MenuId,
            MenuCode,
            Name,
            Description,
            ParentMenuId,
            MenuType,
            Route,
            Icon,
            DisplayOrder,
            IsActive,
            CreatedOn,
            CreatedBy);
}

internal sealed class FgsTenantMenuLookupRow
{
    public long Id { get; set; }
    public int MenuId { get; set; }
    public string MenuCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsTenantMenuLookupDto ToLookupDto() =>
        new(Id, MenuId, MenuCode, Name, DisplayOrder);
}
