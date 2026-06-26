using Fgs.Setup.Application.Features.LeadStatuses.Dtos;

namespace Fgs.Setup.Infrastructure.LeadStatuses;

internal sealed class LeadStatusSummaryRow
{
    public long Id { get; set; }
    public string StatusCode { get; set; }
    public string StatusName { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public LeadStatusSummaryDto ToDto() =>
        new(
            Id,
            StatusCode,
            StatusName,
            Description,
            DisplayOrder,
            IsSystem,
            IsActive);
}

internal sealed class LeadStatusDetailRow
{
    public long Id { get; set; }
    public string StatusCode { get; set; }
    public string StatusName { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public LeadStatusDetailDto ToDto() =>
        new(
            Id,
            StatusCode,
            StatusName,
            Description,
            DisplayOrder,
            IsSystem,
            IsActive);
}

internal sealed class LeadStatusLookupRow
{
    public long Id { get; set; }
    public string StatusCode { get; set; }
    public string StatusName { get; set; }
    public short? DisplayOrder { get; set; }

    public LeadStatusLookupDto ToDto() => new(Id,
            StatusCode,
            StatusName,
            DisplayOrder);
}
