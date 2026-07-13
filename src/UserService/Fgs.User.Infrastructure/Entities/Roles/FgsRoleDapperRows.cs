using Fgs.User.Application.Features.Roles.Dtos;

namespace Fgs.User.Infrastructure.Entities.Roles;

internal sealed class FgsRoleSummaryRow
{
    public long Id { get; set; }

    public string RoleCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long? ParentRoleId { get; set; }

    public bool IsBuiltIn { get; set; }

    public short DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public FgsRoleSummaryDto ToDto() =>
        new(Id, RoleCode, Name, Description, ParentRoleId, IsBuiltIn, DisplayOrder, IsActive);
}

internal sealed class FgsRoleDetailRow
{
    public long Id { get; set; }

    public string RoleCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long? ParentRoleId { get; set; }

    public bool IsBuiltIn { get; set; }

    public short DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public FgsRoleDetailDto ToDto() =>
        new(Id, RoleCode, Name, Description, ParentRoleId, IsBuiltIn, DisplayOrder, IsActive);
}

internal sealed class FgsRoleLookupRow
{
    public long Id { get; set; }

    public string RoleCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsBuiltIn { get; set; }

    public short DisplayOrder { get; set; }

    public FgsRoleLookupDto ToDto() => new(Id, RoleCode, Name, IsBuiltIn, DisplayOrder);
}
