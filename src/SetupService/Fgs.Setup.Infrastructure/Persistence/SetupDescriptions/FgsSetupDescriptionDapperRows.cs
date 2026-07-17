using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupDescriptions;

internal sealed class FgsSetupDescriptionSummaryRow
{
    public long Id { get; set; }
    public string DescriptionTypeCode { get; set; } = null!;
    public string? ShortNote { get; set; }
    public string Body { get; set; } = null!;
    public long? FgsSetupTechTradeId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupDescriptionSummaryDto ToDto() =>
        new(
            Id,
            DescriptionTypeCode,
            ShortNote,
            Body,
            FgsSetupTechTradeId,
            SortOrder,
            IsActive);
}

internal sealed class FgsSetupDescriptionDetailRow
{
    public long Id { get; set; }
    public string DescriptionTypeCode { get; set; } = null!;
    public string? ShortNote { get; set; }
    public string Body { get; set; } = null!;
    public long? FgsSetupTechTradeId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupDescriptionDetailDto ToDto() =>
        new(
            Id,
            DescriptionTypeCode,
            ShortNote,
            Body,
            FgsSetupTechTradeId,
            SortOrder,
            IsActive);
}

internal sealed class FgsSetupDescriptionLookupRow
{
    public long Id { get; set; }
    public string DescriptionTypeCode { get; set; } = null!;
    public string Body { get; set; } = null!;
    public int SortOrder { get; set; }

    public FgsSetupDescriptionLookupDto ToDto() => new(Id,
            DescriptionTypeCode,
            Body,
            SortOrder);
}
