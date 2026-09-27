namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Maps Job Type Tasks to Job Types. A Job Type can contain multiple Job Type Tasks, each with its own display order.
/// </summary>
public class FgsJobTypeCategory : FgsTenantCompanySetupEntityBase<long>
{
    public long JobTypeId { get; set; }

    public long JobTypeTaskId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public FgsJobType? JobType { get; set; }

    public FgsJobTypeTask? JobTypeTask { get; set; }

    public ICollection<FgsJobTypeTask> Tasks { get; set; } = new List<FgsJobTypeTask>();
}
