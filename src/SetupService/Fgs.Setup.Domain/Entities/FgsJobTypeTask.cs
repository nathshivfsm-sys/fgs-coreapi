namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores the tasks that belong to a Job Type Category.
/// </summary>
public class FgsJobTypeTask : FgsTenantCompanySetupEntityBase<long>
{
    public long JobTypeCategoryId { get; set; }

    public long TradeId { get; set; }

    public string TaskName { get; set; } = null!;

    public short Priority { get; set; } = 5;

    public decimal EstimatedHours { get; set; } = 1.00m;

    public short DisplayOrder { get; set; } = 1;

    public FgsJobTypeCategory? JobTypeCategory { get; set; }

    public FgsSetupTechTrade? Trade { get; set; }
}
