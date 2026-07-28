using Fgs.Setup.Domain.Enums;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Defines reusable Job Types that represent the type of work performed.
/// A Job Type serves as the header for one or more Job Type Categories and their associated tasks.
/// </summary>
public class FgsJobType : FgsTenantCompanySetupEntityBase<long>
{
    public string JobTypeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public JobTypeUsedFor UsedFor { get; set; }

    public string? BusinessUnit { get; set; }

    public string? BackgroundColor { get; set; }

    public string? TextColor { get; set; }

    public bool ShowToFieldTech { get; set; } = true;

    public bool ShowOnCustomerPortal { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;

    public ICollection<FgsJobTypeCategory> JobTypeCategories { get; set; } = new List<FgsJobTypeCategory>();
}
