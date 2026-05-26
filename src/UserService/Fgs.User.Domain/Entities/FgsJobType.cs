namespace Fgs.User.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped job/service type used for dispatching, scheduling, and field operations.
/// </summary>
public class FgsJobType : FgsTenantCompanySetupEntityBase<long>
{
    public long JobTypeCategoryId { get; set; }

    public long? JobTypeSubCategoryId { get; set; }

    public string JobTypeCode { get; set; } = null!;

    public string TaskName { get; set; } = null!;

    public string? Description { get; set; }

    public string UsedFor { get; set; } = null!;

    public string? Trade { get; set; }

    public int? EstimatedDurationMinutes { get; set; }

    public string? BusinessUnit { get; set; }

    public short Priority { get; set; } = 5;

    public string? BackgroundColor { get; set; }

    public string? TextColor { get; set; }

    public bool ShowToFieldTech { get; set; } = true;

    public bool ShowOnCustomerPortal { get; set; } = true;

    public short DisplayOrder { get; set; } = 1;

    public FgsJobTypeCategory? JobTypeCategory { get; set; }

    public FgsJobTypeSubCategory? JobTypeSubCategory { get; set; }
}
