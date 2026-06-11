namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Master list of sales pipeline statuses used by Leads and Opportunities.
/// </summary>
public class GloSalesPipelineStatus : GloEntityBase
{
    public short Id { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public string? Description { get; set; }

    public bool AppliesToLead { get; set; }

    public bool AppliesToOpportunity { get; set; }

    public bool IsTerminal { get; set; }

    public bool AllowManualSelection { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;
}
