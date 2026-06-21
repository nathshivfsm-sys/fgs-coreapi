using Fgs.Setup.Application.Features.LeadSources.Dtos;

namespace Fgs.Setup.Infrastructure.LeadSources;

internal sealed class LeadSourceSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string SourceCode { get; set; }
    public string SourceName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public LeadSourceSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            SourceCode,
            SourceName,
            Description,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class LeadSourceDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string SourceCode { get; set; }
    public string SourceName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public LeadSourceDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            SourceCode,
            SourceName,
            Description,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class LeadSourceLookupRow
{
    public long Id { get; set; }
    public string SourceCode { get; set; }
    public string SourceName { get; set; }

    public LeadSourceLookupDto ToDto() => new(Id,
            SourceCode,
            SourceName);
}
