using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.NonWorkingDates;

internal sealed class FgsNonWorkingDateSummaryRow
{
    public long Id { get; set; }
    public DateOnly NonWorkingDate { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }

    public FgsNonWorkingDateSummaryDto ToDto() =>
        new(Id, NonWorkingDate, Name, IsActive);
}

internal sealed class FgsNonWorkingDateDetailRow
{
    public long Id { get; set; }
    public DateOnly NonWorkingDate { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }

    public FgsNonWorkingDateDetailDto ToDto() =>
        new(Id, NonWorkingDate, Name, IsActive);
}

internal sealed class FgsNonWorkingDateLookupRow
{
    public long Id { get; set; }
    public DateOnly NonWorkingDate { get; set; }
    public string Name { get; set; } = null!;

    public FgsNonWorkingDateLookupDto ToDto() =>
        new(Id, NonWorkingDate, Name);
}
