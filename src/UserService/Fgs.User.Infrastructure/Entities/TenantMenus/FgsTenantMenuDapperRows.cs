using Fgs.User.Application.Features.TenantMenus.Dtos;

namespace Fgs.User.Infrastructure.Entities.TenantMenus;

internal sealed class FgsTenantMenuRow
{
    public long Id { get; set; }
    public int MenuId { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }

    public FgsTenantMenuDetailDto ToDetailDto() =>
        new(Id, MenuId, DisplayOrder, IsActive, CreatedOn, CreatedBy);
}
