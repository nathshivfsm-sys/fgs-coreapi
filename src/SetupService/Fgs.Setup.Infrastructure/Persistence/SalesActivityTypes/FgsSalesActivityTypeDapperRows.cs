using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SalesActivityTypes;

internal sealed class FgsSalesActivityTypeSummaryRow
{
    public long Id { get; set; }
    public string ActivityTypeCode { get; set; } = null!;
    public string ActivityTypeName { get; set; } = null!;
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }

    public FgsSalesActivityTypeSummaryDto ToDto() =>
        new(
            Id,
            ActivityTypeCode,
            ActivityTypeName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            AllowManualSelection,
            IsActive);
}

internal sealed class FgsSalesActivityTypeDetailRow
{
    public long Id { get; set; }
    public string ActivityTypeCode { get; set; } = null!;
    public string ActivityTypeName { get; set; } = null!;
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }

    public FgsSalesActivityTypeDetailDto ToDto() =>
        new(
            Id,
            ActivityTypeCode,
            ActivityTypeName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            AllowManualSelection,
            IsActive);
}

internal sealed class FgsSalesActivityTypeLookupRow
{
    public long Id { get; set; }
    public string ActivityTypeCode { get; set; } = null!;
    public string ActivityTypeName { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsSalesActivityTypeLookupDto ToDto() => new(Id,
            ActivityTypeCode,
            ActivityTypeName,
            DisplayOrder);
}
