using Fgs.Setup.Application.Features.Tags.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.Tags;

internal sealed class FgsTagSummaryRow
{
    public long Id { get; set; }
    public string? TagCode { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public long? IconFileId { get; set; }
    public int UsageCount { get; set; }
    public bool IsActive { get; set; }

    public FgsTagSummaryDto ToDto() =>
        new(
            Id,
            TagCode,
            Name,
            Description,
            BackgroundColor,
            TextColor,
            IconFileId,
            UsageCount,
            IsActive);
}

internal sealed class FgsTagDetailRow
{
    public long Id { get; set; }
    public string? TagCode { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public long? IconFileId { get; set; }
    public int UsageCount { get; set; }
    public bool IsActive { get; set; }

    public FgsTagDetailDto ToDto() =>
        new(
            Id,
            TagCode,
            Name,
            Description,
            BackgroundColor,
            TextColor,
            IconFileId,
            UsageCount,
            IsActive);
}

internal sealed class FgsTagLookupRow
{
    public long Id { get; set; }
    public string? TagCode { get; set; }
    public string Name { get; set; } = null!;

    public FgsTagLookupDto ToDto() => new(Id,
            TagCode,
            Name);
}
