using Fgs.Setup.Application.Features.TechTrades.Dtos;

namespace Fgs.Setup.Infrastructure.TechTrades;

/// <summary>
/// Dapper materialization types (settable properties; not positional records).
/// </summary>
internal sealed class TechTradeSummaryRow
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string TradeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public TechTradeSummaryDto ToDto() =>
        new(Id, TenantId, CompanyId, TradeCode, Name, SortOrder, IsActive, CreatedOn, UpdatedOn);
}

internal sealed class TechTradeDetailRow
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string TradeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public TechTradeDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            TradeCode,
            Name,
            Description,
            SortOrder,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class TechTradeLookupRow
{
    public long Id { get; set; }

    public string TradeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? SortOrder { get; set; }

    public TechTradeLookupDto ToDto() => new(Id, TradeCode, Name, SortOrder);
}
