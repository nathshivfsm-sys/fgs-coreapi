using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;

namespace Fgs.Setup.Infrastructure.FgsBusinessTypes;

internal sealed class FgsBusinessTypeSummaryRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsBusinessTypeSummaryDto ToDto() =>
        new(
            Id,
            Code,
            Name,
            Description,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsBusinessTypeDetailRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsBusinessTypeDetailDto ToDto() =>
        new(
            Id,
            Code,
            Name,
            Description,
            DisplayOrder,
            IsActive);
}

internal sealed class FgsBusinessTypeLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short? DisplayOrder { get; set; }

    public FgsBusinessTypeLookupDto ToDto() => new(Id,
            Code,
            Name,
            DisplayOrder);
}
