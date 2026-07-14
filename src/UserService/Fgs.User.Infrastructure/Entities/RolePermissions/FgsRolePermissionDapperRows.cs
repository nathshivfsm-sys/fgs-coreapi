using Fgs.User.Application.Features.RolePermissions.Dtos;

namespace Fgs.User.Infrastructure.Entities.RolePermissions;

internal sealed class FgsRolePermissionRow
{
    public long Id { get; set; }

    public long FgsRoleId { get; set; }

    public long FgsPermissionId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsRolePermissionSummaryDto ToSummaryDto() =>
        new(Id, FgsRoleId, FgsPermissionId, CreatedOn, CreatedBy);

    public FgsRolePermissionDetailDto ToDetailDto() =>
        new(Id, FgsRoleId, FgsPermissionId, CreatedOn, CreatedBy);
}
