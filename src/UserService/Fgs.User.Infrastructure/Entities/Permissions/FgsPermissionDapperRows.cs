using Fgs.User.Application.Features.Permissions.Dtos;

namespace Fgs.User.Infrastructure.Entities.Permissions;

internal sealed class FgsPermissionSummaryRow
{
    public long Id { get; set; }

    public string PermissionCode { get; set; } = null!;

    public string Module { get; set; } = null!;

    public string Resource { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public FgsPermissionSummaryDto ToDto() =>
        new(Id, PermissionCode, Module, Resource, Action, Name, Description, DisplayOrder, IsActive);
}

internal sealed class FgsPermissionDetailRow
{
    public long Id { get; set; }

    public string PermissionCode { get; set; } = null!;

    public string Module { get; set; } = null!;

    public string Resource { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public FgsPermissionDetailDto ToDto() =>
        new(Id, PermissionCode, Module, Resource, Action, Name, Description, DisplayOrder, IsActive);
}

internal sealed class FgsPermissionLookupRow
{
    public long Id { get; set; }

    public string PermissionCode { get; set; } = null!;

    public string Module { get; set; } = null!;

    public string Resource { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string Name { get; set; } = null!;

    public short DisplayOrder { get; set; }

    public FgsPermissionLookupDto ToDto() =>
        new(Id, PermissionCode, Module, Resource, Action, Name, DisplayOrder);
}
