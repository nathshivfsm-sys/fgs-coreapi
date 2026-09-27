namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores the tasks that belong to a Job Category (master catalog).
/// </summary>
public class FgsJobTypeTask : FgsTenantCompanySetupEntityBase<long>
{
    public long JobCategoryId { get; set; }

    public long TradeId { get; set; }

    public long? SkillLevelId { get; set; }

    public string Name { get; set; } = null!;

    public string TaskName { get; set; } = null!;

    public short Priority { get; set; } = 5;

    public decimal EstimatedHours { get; set; } = 1.00m;

    public short DisplayOrder { get; set; } = 1;

    public FgsJobCategory? JobCategory { get; set; }

    public FgsSetupTechTrade? Trade { get; set; }

    public FgsSetupTechSkillLevel? SkillLevel { get; set; }
}
