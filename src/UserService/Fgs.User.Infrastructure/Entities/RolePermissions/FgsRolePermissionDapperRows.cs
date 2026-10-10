using Fgs.User.Application.Features.Permissions.Dtos;
using Fgs.User.Application.Features.RolePermissions.Dtos;

namespace Fgs.User.Infrastructure.Entities.RolePermissions;

internal sealed class FgsRolePermissionRow
{
    public long Id { get; set; }

    public long FgsRoleId { get; set; }

    public long FgsPermissionId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public long? PermissionId { get; set; }

    public string? PermissionCode { get; set; }

    public string? Module { get; set; }

    public string? Resource { get; set; }

    public string? Action { get; set; }

    public string? PermissionName { get; set; }

    public string? PermissionDescription { get; set; }

    public short? PermissionDisplayOrder { get; set; }

    public bool? PermissionIsActive { get; set; }

    public FgsRolePermissionSummaryDto ToSummaryDto() =>
        new(Id, FgsRoleId, FgsPermissionId, CreatedOn, CreatedBy);

    public FgsRolePermissionDetailDto ToDetailDto() =>
        new(Id, FgsRoleId, FgsPermissionId, CreatedOn, CreatedBy, ToPermission());

    private FgsPermissionDetailDto? ToPermission()
    {
        if (PermissionId is null
            || PermissionCode is null
            || Module is null
            || Resource is null
            || Action is null
            || PermissionName is null)
        {
            return null;
        }

        return new FgsPermissionDetailDto(
            PermissionId.Value,
            PermissionCode,
            Module,
            Resource,
            Action,
            PermissionName,
            PermissionDescription,
            PermissionDisplayOrder ?? 1,
            PermissionIsActive ?? true);
    }
}

internal sealed class FgsRolePermissionLookupRow
{
    public long Id { get; set; }

    public long FgsRoleId { get; set; }

    public long FgsPermissionId { get; set; }

    public FgsRolePermissionLookupDto ToLookupDto() => new(Id, FgsRoleId, FgsPermissionId);
}
