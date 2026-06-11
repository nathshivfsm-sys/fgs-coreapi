namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Master list of sales disposition reasons used when a Lead is Disqualified or an Opportunity is Lost.
/// </summary>
public class GloSalesDispositionReason : GloEntityBase
{
    public short Id { get; set; }

    public string DispositionReasonCode { get; set; } = null!;

    public string DispositionReasonName { get; set; } = null!;

    public string? Description { get; set; }

    public bool AppliesToLead { get; set; }

    public bool AppliesToOpportunity { get; set; }

    public bool RequireComment { get; set; }

    public bool IsTerminal { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;
}
