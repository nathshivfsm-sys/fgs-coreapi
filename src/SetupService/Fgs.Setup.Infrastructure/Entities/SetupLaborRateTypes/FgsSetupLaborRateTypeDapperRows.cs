using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.SetupLaborRateTypes;

internal sealed class FgsSetupLaborRateTypeSummaryRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupLaborRateTypeSummaryDto ToDto() =>
        new(
            Id,
            Name,
            Description,
            SortOrder,
            IsActive);
}

internal sealed class FgsSetupLaborRateTypeDetailRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupLaborRateTypeDetailDto ToDto() =>
        new(
            Id,
            Name,
            Description,
            SortOrder,
            IsActive);
}

internal sealed class FgsSetupLaborRateTypeLookupRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public int SortOrder { get; set; }

    public FgsSetupLaborRateTypeLookupDto ToDto() => new(Id,
            Name,
            SortOrder);
}
