using Fgs.Setup.Application.Features.LeadSources.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.LeadSources;

internal sealed class LeadSourceSummaryRow
{
    public long Id { get; set; }
    public string SourceCode { get; set; } = null!;
    public string SourceName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public LeadSourceSummaryDto ToDto() =>
        new(
            Id,
            SourceCode,
            SourceName,
            Description,
            IsActive);
}

internal sealed class LeadSourceDetailRow
{
    public long Id { get; set; }
    public string SourceCode { get; set; } = null!;
    public string SourceName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public LeadSourceDetailDto ToDto() =>
        new(
            Id,
            SourceCode,
            SourceName,
            Description,
            IsActive);
}

internal sealed class LeadSourceLookupRow
{
    public long Id { get; set; }
    public string SourceCode { get; set; } = null!;
    public string SourceName { get; set; } = null!;

    public LeadSourceLookupDto ToDto() => new(Id,
            SourceCode,
            SourceName);
}
