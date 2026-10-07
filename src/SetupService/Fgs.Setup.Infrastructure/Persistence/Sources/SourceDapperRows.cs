using Fgs.Setup.Application.Features.Sources.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.Sources;

internal sealed class SourcesummaryRow
{
    public long Id { get; set; }
    public string SourceCode { get; set; } = null!;
    public string SourceName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public SourcesummaryDto ToDto() =>
        new(
            Id,
            SourceCode,
            SourceName,
            Description,
            IsActive);
}

internal sealed class SourceDetailRow
{
    public long Id { get; set; }
    public string SourceCode { get; set; } = null!;
    public string SourceName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public SourceDetailDto ToDto() =>
        new(
            Id,
            SourceCode,
            SourceName,
            Description,
            IsActive);
}

internal sealed class SourceLookupRow
{
    public long Id { get; set; }
    public string SourceCode { get; set; } = null!;
    public string SourceName { get; set; } = null!;

    public SourceLookupDto ToDto() => new(Id,
            SourceCode,
            SourceName);
}
