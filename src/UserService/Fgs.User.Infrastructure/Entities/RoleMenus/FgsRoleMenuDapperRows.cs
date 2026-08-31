using Fgs.User.Application.Features.RoleMenus.Dtos;

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

    public FgsRoleMenuDetailDto ToDetailDto() =>
        new(Id, RoleId, MenuId, DisplayOrder, IsActive, CreatedOn, CreatedBy);
}
