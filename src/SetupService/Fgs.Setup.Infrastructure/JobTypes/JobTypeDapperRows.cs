using Fgs.Setup.Application.Features.JobTypes.Dtos;

namespace Fgs.Setup.Infrastructure.JobTypes;

internal sealed class JobTypeSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long JobTypeCategoryId { get; set; }
    public long? JobTypeSubCategoryId { get; set; }
    public string JobTypeCode { get; set; }
    public string TaskName { get; set; }
    public string? Description { get; set; }
    public string UsedFor { get; set; }
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
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public JobTypeSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
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
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class JobTypeDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long JobTypeCategoryId { get; set; }
    public long? JobTypeSubCategoryId { get; set; }
    public string JobTypeCode { get; set; }
    public string TaskName { get; set; }
    public string? Description { get; set; }
    public string UsedFor { get; set; }
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
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public JobTypeDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
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
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class JobTypeLookupRow
{
    public long Id { get; set; }
    public string JobTypeCode { get; set; }
    public string TaskName { get; set; }
    public short? DisplayOrder { get; set; }

    public JobTypeLookupDto ToDto() => new(Id,
            JobTypeCode,
            TaskName,
            DisplayOrder);
}
