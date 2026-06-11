namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Master list of sales activity types used by Leads and Opportunities.
/// </summary>
public class GloSalesActivityType : GloEntityBase
{
    public short Id { get; set; }

    public string ActivityTypeCode { get; set; } = null!;

    public string ActivityTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool AppliesToLead { get; set; } = true;

    public bool AppliesToOpportunity { get; set; } = true;

    public bool AllowManualSelection { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;
}
