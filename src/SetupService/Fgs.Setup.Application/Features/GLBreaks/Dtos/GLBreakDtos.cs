using Fgs.Setup.Application.Common.Locations;

namespace Fgs.Setup.Application.Features.GLBreaks.Dtos;

public sealed record GLBreakTradeDto(
    long Id,
    string TradeCode);

public sealed record GLBreakAddressDetailDto(
    Guid Id,
    string? AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? FormattedAddress,
    decimal? Latitude,
    decimal? Longitude);

public sealed record GLBreakSummaryDto(
    long Id,
    string Code,
    string Name,
    string? BreakLabel,
    short BreakLevel,
    long? LogoFileId,
    bool IsActive);

public sealed record GLBreakDetailDto(
    long Id,
    string Code,
    string Name,
    string? BreakLabel,
    short BreakLevel,
    long? LogoFileId,
    GLBreakAddressDetailDto? Address,
    IReadOnlyList<GLBreakTradeDto> Trades,
    bool IsActive);

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
