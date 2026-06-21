using Fgs.Setup.Application.Features.Tags.Dtos;

namespace Fgs.Setup.Infrastructure.Tags;

internal sealed class FgsTagSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string? TagCode { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public long? IconFileId { get; set; }
    public int UsageCount { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsTagSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            TagCode,
            Name,
            Description,
            BackgroundColor,
            TextColor,
            IconFileId,
            UsageCount,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsTagDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string? TagCode { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public long? IconFileId { get; set; }
    public int UsageCount { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsTagDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            TagCode,
            Name,
            Description,
            BackgroundColor,
            TextColor,
            IconFileId,
            UsageCount,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsTagLookupRow
{
    public long Id { get; set; }
    public string? TagCode { get; set; }
    public string Name { get; set; }

    public FgsTagLookupDto ToDto() => new(Id,
            TagCode,
            Name);
}
