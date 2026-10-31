namespace Fgs.Setup.Application.Features.PriceBooks.Dtos;

public sealed record FgsPriceBookSummaryDto(
    long Id,
    string PriceBookCode,
    string PriceBookName,
    long? PriceBookVersionId,
    string? Version,
    DateOnly? EffectiveFrom,
    DateOnly? EffectiveTo,
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
    long? PriceBookVersionId,
    string? Version,
    DateOnly? EffectiveFrom,
    DateOnly? EffectiveTo,
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
    string PricingModel,
    long? PriceBookVersionId);

public sealed record FgsPriceBookCreateDto(
    string PriceBookCode,
    string PriceBookName,
    string? Description,
    long JobTypeId,
    string PricingModel,
    int EstimatedDurationMinutes,
    decimal? BasePrice,
    bool IsTaxable = true,
    long? PriceBookVersionId = null);

public sealed record FgsPriceBookUpdateDto(
    string PriceBookCode,
    string PriceBookName,
    string? Description,
    long JobTypeId,
    string PricingModel,
    int EstimatedDurationMinutes,
    decimal? BasePrice,
    bool IsTaxable,
    long? PriceBookVersionId = null);

public sealed record FgsPriceBookPatchDto(
    string? PriceBookCode = null,
    string? PriceBookName = null,
    string? Description = null,
    long? JobTypeId = null,
    string? PricingModel = null,
    int? EstimatedDurationMinutes = null,
    decimal? BasePrice = null,
    bool? IsTaxable = null,
    bool? IsActive = null,
    long? PriceBookVersionId = null);

public sealed record FgsPriceBookListFilters(
    long? JobTypeId = null,
    string? PricingModel = null,
    string? PriceBookCode = null,
    string? PriceBookName = null,
    long? PriceBookVersionId = null);
