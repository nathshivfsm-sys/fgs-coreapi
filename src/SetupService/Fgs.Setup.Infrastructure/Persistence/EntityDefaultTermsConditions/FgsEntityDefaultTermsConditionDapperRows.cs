using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.EntityDefaultTermsConditions;

internal sealed class FgsEntityDefaultTermsConditionSummaryRow
{
    public long Id { get; set; }
    public string EntityType { get; set; } = null!;
    public long TermsConditionId { get; set; }
    public string? TermsConditionCode { get; set; }
    public string? TermsConditionName { get; set; }
    public int? TermsConditionVersionNumber { get; set; }
    public bool IsActive { get; set; }

    public FgsEntityDefaultTermsConditionSummaryDto ToDto() =>
        new(
            Id,
            EntityType,
            TermsConditionId,
            TermsConditionCode,
            TermsConditionName,
            TermsConditionVersionNumber,
            IsActive);
}

internal sealed class FgsEntityDefaultTermsConditionDetailRow
{
    public long Id { get; set; }
    public string EntityType { get; set; } = null!;
    public long TermsConditionId { get; set; }
    public string? TermsConditionCode { get; set; }
    public string? TermsConditionName { get; set; }
    public int? TermsConditionVersionNumber { get; set; }
    public bool IsActive { get; set; }

    public FgsEntityDefaultTermsConditionDetailDto ToDto() =>
        new(
            Id,
            EntityType,
            TermsConditionId,
            TermsConditionCode,
            TermsConditionName,
            TermsConditionVersionNumber,
            IsActive);
}

internal sealed class FgsEntityDefaultTermsConditionLookupRow
{
    public long Id { get; set; }
    public string EntityType { get; set; } = null!;
    public long TermsConditionId { get; set; }

    public FgsEntityDefaultTermsConditionLookupDto ToDto() =>
        new(Id, EntityType, TermsConditionId);
}
