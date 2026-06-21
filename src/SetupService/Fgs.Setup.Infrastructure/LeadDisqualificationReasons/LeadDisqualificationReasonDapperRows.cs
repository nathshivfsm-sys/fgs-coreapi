using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;

namespace Fgs.Setup.Infrastructure.LeadDisqualificationReasons;

internal sealed class LeadDisqualificationReasonSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string ReasonCode { get; set; }
    public string ReasonName { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public LeadDisqualificationReasonSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            ReasonCode,
            ReasonName,
            Description,
            DisplayOrder,
            IsSystem,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class LeadDisqualificationReasonDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string ReasonCode { get; set; }
    public string ReasonName { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public LeadDisqualificationReasonDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            ReasonCode,
            ReasonName,
            Description,
            DisplayOrder,
            IsSystem,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class LeadDisqualificationReasonLookupRow
{
    public long Id { get; set; }
    public string ReasonCode { get; set; }
    public string ReasonName { get; set; }
    public short? DisplayOrder { get; set; }

    public LeadDisqualificationReasonLookupDto ToDto() => new(Id,
            ReasonCode,
            ReasonName,
            DisplayOrder);
}
