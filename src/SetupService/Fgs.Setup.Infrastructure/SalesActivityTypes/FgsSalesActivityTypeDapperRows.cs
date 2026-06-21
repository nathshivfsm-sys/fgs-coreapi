using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;

namespace Fgs.Setup.Infrastructure.SalesActivityTypes;

internal sealed class FgsSalesActivityTypeSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string ActivityTypeCode { get; set; }
    public string ActivityTypeName { get; set; }
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSalesActivityTypeSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            ActivityTypeCode,
            ActivityTypeName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            AllowManualSelection,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSalesActivityTypeDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string ActivityTypeCode { get; set; }
    public string ActivityTypeName { get; set; }
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSalesActivityTypeDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            ActivityTypeCode,
            ActivityTypeName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            AllowManualSelection,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSalesActivityTypeLookupRow
{
    public long Id { get; set; }
    public string ActivityTypeCode { get; set; }
    public string ActivityTypeName { get; set; }
    public short DisplayOrder { get; set; }

    public FgsSalesActivityTypeLookupDto ToDto() => new(Id,
            ActivityTypeCode,
            ActivityTypeName,
            DisplayOrder);
}
