namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Master list of sales activity outcomes used by Leads and Opportunities.
/// </summary>
public class GloSalesActivityOutcome : GloEntityBase
{
    public short Id { get; set; }

    public string OutcomeCode { get; set; } = null!;

    public string OutcomeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool AppliesToLead { get; set; } = true;

    public bool AppliesToOpportunity { get; set; } = true;

    public string? NextSalesPipelineStatusCode { get; set; }

    public bool IsTerminal { get; set; }

    public bool RequireComment { get; set; }

    public bool AllowManualSelection { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;
}
