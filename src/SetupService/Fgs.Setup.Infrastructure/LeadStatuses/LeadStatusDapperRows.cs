using Fgs.Setup.Application.Features.LeadStatuses.Dtos;

namespace Fgs.Setup.Infrastructure.LeadStatuses;

internal sealed class LeadStatusSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string StatusCode { get; set; }
    public string StatusName { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public LeadStatusSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            StatusCode,
            StatusName,
            Description,
            DisplayOrder,
            IsSystem,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class LeadStatusDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string StatusCode { get; set; }
    public string StatusName { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public LeadStatusDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            StatusCode,
            StatusName,
            Description,
            DisplayOrder,
            IsSystem,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class LeadStatusLookupRow
{
    public long Id { get; set; }
    public string StatusCode { get; set; }
    public string StatusName { get; set; }
    public short? DisplayOrder { get; set; }

    public LeadStatusLookupDto ToDto() => new(Id,
            StatusCode,
            StatusName,
            DisplayOrder);
}
