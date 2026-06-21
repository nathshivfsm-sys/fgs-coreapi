using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;

namespace Fgs.Setup.Infrastructure.SetupDescriptions;

internal sealed class FgsSetupDescriptionSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string DescriptionTypeCode { get; set; }
    public string? ShortNote { get; set; }
    public string Body { get; set; }
    public long? FgsSetupTechTradeId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupDescriptionSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            DescriptionTypeCode,
            ShortNote,
            Body,
            FgsSetupTechTradeId,
            SortOrder,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupDescriptionDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string DescriptionTypeCode { get; set; }
    public string? ShortNote { get; set; }
    public string Body { get; set; }
    public long? FgsSetupTechTradeId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupDescriptionDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            DescriptionTypeCode,
            ShortNote,
            Body,
            FgsSetupTechTradeId,
            SortOrder,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupDescriptionLookupRow
{
    public long Id { get; set; }
    public string DescriptionTypeCode { get; set; }
    public string Body { get; set; }
    public int SortOrder { get; set; }

    public FgsSetupDescriptionLookupDto ToDto() => new(Id,
            DescriptionTypeCode,
            Body,
            SortOrder);
}
