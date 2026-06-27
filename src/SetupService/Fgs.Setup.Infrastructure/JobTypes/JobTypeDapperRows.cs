using Fgs.Setup.Application.Features.JobTypes.Dtos;

namespace Fgs.Setup.Infrastructure.JobTypes;

internal sealed class JobTypeSummaryRow
{
    public long Id { get; set; }
    public long JobTypeCategoryId { get; set; }
    public long? JobTypeSubCategoryId { get; set; }
    public string JobTypeCode { get; set; } = null!;
    public string TaskName { get; set; } = null!;
    public string? Description { get; set; }
    public string UsedFor { get; set; } = null!;
    public string? Trade { get; set; }
    public int? EstimatedDurationMinutes { get; set; }
    public string? BusinessUnit { get; set; }
    public short Priority { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool ShowOnCustomerPortal { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeSummaryDto ToDto() =>
        new(
            Id,
            JobTypeCategoryId,
            JobTypeSubCategoryId,
            JobTypeCode,
            TaskName,
            Description,
            UsedFor,
            Trade,
            EstimatedDurationMinutes,
            BusinessUnit,
            Priority,
            BackgroundColor,
            TextColor,
            ShowToFieldTech,
            ShowOnCustomerPortal,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeDetailRow
{
    public long Id { get; set; }
    public long JobTypeCategoryId { get; set; }
    public long? JobTypeSubCategoryId { get; set; }
    public string JobTypeCode { get; set; } = null!;
    public string TaskName { get; set; } = null!;
    public string? Description { get; set; }
    public string UsedFor { get; set; } = null!;
    public string? Trade { get; set; }
    public int? EstimatedDurationMinutes { get; set; }
    public string? BusinessUnit { get; set; }
    public short Priority { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool ShowOnCustomerPortal { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeDetailDto ToDto() =>
        new(
            Id,
            JobTypeCategoryId,
            JobTypeSubCategoryId,
            JobTypeCode,
            TaskName,
            Description,
            UsedFor,
            Trade,
            EstimatedDurationMinutes,
            BusinessUnit,
            Priority,
            BackgroundColor,
            TextColor,
            ShowToFieldTech,
            ShowOnCustomerPortal,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeLookupRow
{
    public long Id { get; set; }
    public string JobTypeCode { get; set; } = null!;
    public string TaskName { get; set; } = null!;
    public short? DisplayOrder { get; set; }

    public JobTypeLookupDto ToDto() => new(Id,
            JobTypeCode,
            TaskName,
            DisplayOrder);
}
