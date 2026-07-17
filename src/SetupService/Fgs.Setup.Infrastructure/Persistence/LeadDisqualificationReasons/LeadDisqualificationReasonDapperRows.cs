using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.LeadDisqualificationReasons;

internal sealed class LeadDisqualificationReasonSummaryRow
{
    public long Id { get; set; }
    public string ReasonCode { get; set; } = null!;
    public string ReasonName { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public LeadDisqualificationReasonSummaryDto ToDto() =>
        new(
            Id,
            ReasonCode,
            ReasonName,
            Description,
            DisplayOrder,
            IsSystem,
            IsActive);
}

internal sealed class LeadDisqualificationReasonDetailRow
{
    public long Id { get; set; }
    public string ReasonCode { get; set; } = null!;
    public string ReasonName { get; set; } = null!;
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public LeadDisqualificationReasonDetailDto ToDto() =>
        new(
            Id,
            ReasonCode,
            ReasonName,
            Description,
            DisplayOrder,
            IsSystem,
            IsActive);
}

internal sealed class LeadDisqualificationReasonLookupRow
{
    public long Id { get; set; }
    public string ReasonCode { get; set; } = null!;
    public string ReasonName { get; set; } = null!;
    public short? DisplayOrder { get; set; }

    public LeadDisqualificationReasonLookupDto ToDto() => new(Id,
            ReasonCode,
            ReasonName,
            DisplayOrder);
}
