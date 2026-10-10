using Fgs.User.Application.Features.RoleMenus.Dtos;
using Fgs.User.Application.Features.TenantMenus.Dtos;

namespace Fgs.User.Infrastructure.Entities.RoleMenus;

internal sealed class FgsRoleMenuRow
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public int MenuId { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public long? TenantMenuId { get; set; }
    public string? MenuCode { get; set; }
    public string? MenuName { get; set; }
    public string? MenuDescription { get; set; }
    public int? ParentMenuId { get; set; }
    public string? MenuType { get; set; }
    public string? Route { get; set; }
    public string? Icon { get; set; }
    public short? MenuDisplayOrder { get; set; }
    public bool? MenuIsActive { get; set; }
    public DateTimeOffset? MenuCreatedOn { get; set; }
    public string? MenuCreatedBy { get; set; }

    public FgsRoleMenuDetailDto ToDetailDto() =>
        new(Id, RoleId, MenuId, DisplayOrder, IsActive, CreatedOn, CreatedBy, ToMenu());

    private FgsTenantMenuDetailDto? ToMenu()
    {
        if (TenantMenuId is null || MenuCode is null || MenuName is null || MenuType is null)
        {
            return null;
        }

        return new FgsTenantMenuDetailDto(
            TenantMenuId.Value,
            MenuId,
            MenuCode,
            MenuName,
            MenuDescription,
            ParentMenuId,
            MenuType,
            Route,
            Icon,
            MenuDisplayOrder ?? 1,
            MenuIsActive ?? true,
            MenuCreatedOn ?? CreatedOn,
            MenuCreatedBy);
    }
}

internal sealed class FgsRoleMenuLookupRow
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public int MenuId { get; set; }
    public short DisplayOrder { get; set; }

    public FgsRoleMenuLookupDto ToLookupDto() =>
        new(Id, RoleId, MenuId, DisplayOrder);
}
