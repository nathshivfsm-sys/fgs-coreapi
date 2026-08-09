namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Maps Job Categories to Job Types. A Job Type can contain one or more Job Categories.
/// </summary>
public class FgsJobTypeCategory : FgsTenantCompanySetupEntityBase<long>
{
    public long JobTypeId { get; set; }

    public long JobCategoryId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public FgsJobType? JobType { get; set; }

    public FgsJobCategory? JobCategory { get; set; }

    public ICollection<FgsJobTypeTask> Tasks { get; set; } = new List<FgsJobTypeTask>();
}
