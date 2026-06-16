namespace Fgs.Setup.Application.Features.TechTrades.Dtos;

public sealed record TechTradeSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string TradeCode,
    string Name,
    int? SortOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record TechTradeDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string TradeCode,
    string Name,
    string? Description,
    int? SortOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record TechTradeLookupDto(
    long Id,
    string TradeCode,
    string Name,
    int? SortOrder);

public sealed record TechTradeCreateDto(
    string TradeCode,
    string Name,
    string? Description,
    int? SortOrder);

public sealed record TechTradeUpdateDto(
    string TradeCode,
    string Name,
    string? Description,
    int? SortOrder);

public sealed record TechTradePatchDto(
    string? TradeCode,
    string? Name,
    string? Description,
    int? SortOrder,
    bool? IsActive);

public sealed record TechTradeListFilters(
    string? TradeCode = null,
    string? Name = null);
