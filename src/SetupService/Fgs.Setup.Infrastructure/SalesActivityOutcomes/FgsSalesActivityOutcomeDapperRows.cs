using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;

namespace Fgs.Setup.Infrastructure.SalesActivityOutcomes;

internal sealed class FgsSalesActivityOutcomeSummaryRow
{
    public long Id { get; set; }
    public string OutcomeCode { get; set; }
    public string OutcomeName { get; set; }
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public long? NextSalesPipelineStatusId { get; set; }
    public bool IsTerminal { get; set; }
    public bool RequireComment { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }

    public FgsSalesActivityOutcomeSummaryDto ToDto() =>
        new(
            Id,
            OutcomeCode,
            OutcomeName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            NextSalesPipelineStatusId,
            IsTerminal,
            RequireComment,
            AllowManualSelection,
            IsActive);
}

internal sealed class FgsSalesActivityOutcomeDetailRow
{
    public long Id { get; set; }
    public string OutcomeCode { get; set; }
    public string OutcomeName { get; set; }
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public long? NextSalesPipelineStatusId { get; set; }
    public bool IsTerminal { get; set; }
    public bool RequireComment { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }

    public FgsSalesActivityOutcomeDetailDto ToDto() =>
        new(
            Id,
            OutcomeCode,
            OutcomeName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            NextSalesPipelineStatusId,
            IsTerminal,
            RequireComment,
            AllowManualSelection,
            IsActive);
}

internal sealed class FgsSalesActivityOutcomeLookupRow
{
    public long Id { get; set; }
    public string OutcomeCode { get; set; }
    public string OutcomeName { get; set; }
    public short DisplayOrder { get; set; }

    public FgsSalesActivityOutcomeLookupDto ToDto() => new(Id,
            OutcomeCode,
            OutcomeName,
            DisplayOrder);
}
