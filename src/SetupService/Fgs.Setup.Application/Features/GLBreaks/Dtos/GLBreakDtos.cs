using Fgs.Setup.Application.Common.Locations;

namespace Fgs.Setup.Application.Features.GLBreaks.Dtos;

public sealed record GLBreakTradeDto(
    long Id,
    long GLBreakId,
    string TradeCode,
    DateTimeOffset CreatedOn,
    string? CreatedBy);

public sealed record GLBreakSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Code,
    string Name,
    string? BreakLabel,
    short BreakLevel,
    long? LogoFileId,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record GLBreakDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Code,
    string Name,
    string? BreakLabel,
    short BreakLevel,
    long? LogoFileId,
    LocationDetailDto? Address,
    IReadOnlyList<GLBreakTradeDto> Trades,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record GLBreakLookupDto(
    long Id,
    string Code,
    string Name,
    short BreakLevel);

public sealed record GLBreakCreateDto(
    string Code,
    string Name,
    string? BreakLabel,
    short BreakLevel,
    long? LogoFileId,
    LocationWriteDto? Address,
    IReadOnlyList<string> TradeCodes);

public sealed record GLBreakUpdateDto(
    string Code,
    string Name,
    string? BreakLabel,
    short BreakLevel,
    long? LogoFileId,
    LocationWriteDto? Address,
    IReadOnlyList<string> TradeCodes);

public sealed record GLBreakPatchDto(
    string? Code,
    string? Name,
    string? BreakLabel,
    short? BreakLevel,
    long? LogoFileId,
    LocationWriteDto? Address,
    IReadOnlyList<string>? TradeCodes,
    bool? IsActive);

public sealed record GLBreakListFilters(
    string? Code = null,
    string? Name = null,
    short? BreakLevel = null,
    string? TradeCode = null);
