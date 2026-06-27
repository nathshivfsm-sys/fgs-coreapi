using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;

namespace Fgs.Setup.Infrastructure.SalesDispositionReasons;

internal sealed class FgsSalesDispositionReasonSummaryRow
{
    public long Id { get; set; }
    public string DispositionReasonCode { get; set; } = null!;
    public string DispositionReasonName { get; set; } = null!;
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool RequireComment { get; set; }
    public bool IsTerminal { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }

    public FgsSalesDispositionReasonSummaryDto ToDto() =>
        new(
            Id,
            DispositionReasonCode,
            DispositionReasonName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            RequireComment,
            IsTerminal,
            AllowManualSelection,
            IsActive);
}

internal sealed class FgsSalesDispositionReasonDetailRow
{
    public long Id { get; set; }
    public string DispositionReasonCode { get; set; } = null!;
    public string DispositionReasonName { get; set; } = null!;
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool RequireComment { get; set; }
    public bool IsTerminal { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }

    public FgsSalesDispositionReasonDetailDto ToDto() =>
        new(
            Id,
            DispositionReasonCode,
            DispositionReasonName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            RequireComment,
            IsTerminal,
            AllowManualSelection,
            IsActive);
}

internal sealed class FgsSalesDispositionReasonLookupRow
{
    public long Id { get; set; }
    public string DispositionReasonCode { get; set; } = null!;
    public string DispositionReasonName { get; set; } = null!;
    public short DisplayOrder { get; set; }

    public FgsSalesDispositionReasonLookupDto ToDto() => new(Id,
            DispositionReasonCode,
            DispositionReasonName,
            DisplayOrder);
}
