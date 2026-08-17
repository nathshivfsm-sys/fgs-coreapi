namespace Fgs.Setup.Application.Features.PriceBooks.Dtos;

public sealed record FgsPriceBookSummaryDto(
    long Id,
    string PriceBookCode,
    string PriceBookName,
    long JobTypeId,
    string PricingModel,
    int EstimatedDurationMinutes,
    decimal? BasePrice,
    bool IsTaxable,
    bool IsActive);

public sealed record FgsPriceBookDetailDto(
    long Id,
    string PriceBookCode,
    string PriceBookName,
    string? Description,
    long JobTypeId,
    string PricingModel,
    int EstimatedDurationMinutes,
    decimal? BasePrice,
    bool IsTaxable,
    bool IsActive);

public sealed record FgsPriceBookLookupDto(
    long Id,
    string PriceBookCode,
    string PriceBookName,
    string PricingModel);

public sealed record FgsPriceBookCreateDto(
    string PriceBookCode,
    string PriceBookName,
    string? Description,
    long JobTypeId,
    string PricingModel,
    int EstimatedDurationMinutes,
    decimal? BasePrice,
    bool IsTaxable = true);

public sealed record FgsPriceBookUpdateDto(
    string PriceBookCode,
    string PriceBookName,
    string? Description,
    long JobTypeId,
    string PricingModel,
    int EstimatedDurationMinutes,
    decimal? BasePrice,
    bool IsTaxable);

public sealed record FgsPriceBookPatchDto(
    string? PriceBookCode = null,
    string? PriceBookName = null,
    string? Description = null,
    long? JobTypeId = null,
    string? PricingModel = null,
    int? EstimatedDurationMinutes = null,
    decimal? BasePrice = null,
    bool? IsTaxable = null,
    bool? IsActive = null);

public sealed record FgsPriceBookListFilters(
    long? JobTypeId = null,
    string? PricingModel = null,
    string? PriceBookCode = null,
    string? PriceBookName = null);
