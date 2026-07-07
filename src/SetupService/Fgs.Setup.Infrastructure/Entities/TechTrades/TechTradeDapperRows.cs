using Fgs.Setup.Application.Features.TechTrades.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.TechTrades;

/// <summary>
/// Dapper materialization types (settable properties; not positional records).
/// </summary>
internal sealed class TechTradeSummaryRow
{
    public long Id { get; set; }

    public string TradeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool IsActive { get; set; }

    public TechTradeSummaryDto ToDto() =>
        new(Id, TradeCode, Name, SortOrder, IsActive);
}

internal sealed class TechTradeDetailRow
{
    public long Id { get; set; }

    public string TradeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? SortOrder { get; set; }

    public bool IsActive { get; set; }

    public TechTradeDetailDto ToDto() =>
        new(Id, TradeCode, Name, Description, SortOrder, IsActive);
}

internal sealed class TechTradeLookupRow
{
    public long Id { get; set; }

    public string TradeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? SortOrder { get; set; }

    public TechTradeLookupDto ToDto() => new(Id, TradeCode, Name, SortOrder);
}
