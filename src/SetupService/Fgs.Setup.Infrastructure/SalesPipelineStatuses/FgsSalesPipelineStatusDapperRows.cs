using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;

namespace Fgs.Setup.Infrastructure.SalesPipelineStatuses;

internal sealed class FgsSalesPipelineStatusSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string StatusCode { get; set; }
    public string StatusName { get; set; }
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool IsTerminal { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSalesPipelineStatusSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            StatusCode,
            StatusName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            IsTerminal,
            AllowManualSelection,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSalesPipelineStatusDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string StatusCode { get; set; }
    public string StatusName { get; set; }
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool IsTerminal { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSalesPipelineStatusDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            StatusCode,
            StatusName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            IsTerminal,
            AllowManualSelection,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSalesPipelineStatusLookupRow
{
    public long Id { get; set; }
    public string StatusCode { get; set; }
    public string StatusName { get; set; }
    public short DisplayOrder { get; set; }

    public FgsSalesPipelineStatusLookupDto ToDto() => new(Id,
            StatusCode,
            StatusName,
            DisplayOrder);
}
