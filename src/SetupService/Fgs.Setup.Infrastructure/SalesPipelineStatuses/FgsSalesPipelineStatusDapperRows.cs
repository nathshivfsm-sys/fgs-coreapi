using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;

namespace Fgs.Setup.Infrastructure.SalesPipelineStatuses;

internal sealed class FgsSalesPipelineStatusSummaryRow
{
    public long Id { get; set; }
    public string StatusCode { get; set; } = null!;
    public string StatusName { get; set; } = null!;
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool IsTerminal { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }

    public FgsSalesPipelineStatusSummaryDto ToDto() =>
        new(
            Id,
            StatusCode,
            StatusName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            IsTerminal,
            AllowManualSelection,
            IsActive);
}

internal sealed class FgsSalesPipelineStatusDetailRow
{
    public long Id { get; set; }
    public string StatusCode { get; set; } = null!;
    public string StatusName { get; set; } = null!;
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool IsTerminal { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }

    public FgsSalesPipelineStatusDetailDto ToDto() =>
        new(
            Id,
            StatusCode,
            StatusName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            IsTerminal,
            AllowManualSelection,
            IsActive);
}

internal sealed class FgsSalesPipelineStatusLookupRow
{
    public long Id { get; set; }
    public string StatusCode { get; set; } = null!;
    public string StatusName { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsSalesPipelineStatusLookupDto ToDto() => new(Id,
            StatusCode,
            StatusName,
            DisplayOrder);
}
