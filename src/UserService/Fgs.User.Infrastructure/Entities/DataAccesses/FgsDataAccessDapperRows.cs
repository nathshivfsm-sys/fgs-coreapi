using Fgs.User.Application.Features.DataAccesses.Dtos;

namespace Fgs.User.Infrastructure.Entities.DataAccesses;

internal sealed class FgsDataAccessSummaryRow
{
    public long Id { get; set; }

    public string DataAccessCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsBuiltIn { get; set; }

    public short DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public FgsDataAccessSummaryDto ToDto() =>
        new(Id, DataAccessCode, Name, Description, IsBuiltIn, DisplayOrder, IsActive);
}

internal sealed class FgsDataAccessDetailRow
{
    public long Id { get; set; }

    public string DataAccessCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsBuiltIn { get; set; }

    public short DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public FgsDataAccessDetailDto ToDto() =>
        new(Id, DataAccessCode, Name, Description, IsBuiltIn, DisplayOrder, IsActive);
}

internal sealed class FgsDataAccessLookupRow
{
    public long Id { get; set; }

    public string DataAccessCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsBuiltIn { get; set; }

    public short DisplayOrder { get; set; }

    public FgsDataAccessLookupDto ToDto() => new(Id, DataAccessCode, Name, IsBuiltIn, DisplayOrder);
}
