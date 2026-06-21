using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;

namespace Fgs.Setup.Infrastructure.TitlesOfCourtesy;

/// <summary>
/// Dapper materialization types (settable properties; not positional records).
/// </summary>
internal sealed class TitleOfCourtesySummaryRow
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public TitleOfCourtesySummaryDto ToDto() =>
        new(Id, TenantId, CompanyId, Code, DisplayName, SortOrder, IsActive, CreatedOn, UpdatedOn);
}

internal sealed class TitleOfCourtesyDetailRow
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public TitleOfCourtesyDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            DisplayName,
            SortOrder,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class TitleOfCourtesyLookupRow
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int? SortOrder { get; set; }

    public TitleOfCourtesyLookupDto ToDto() => new(Id, Code, DisplayName, SortOrder);
}
