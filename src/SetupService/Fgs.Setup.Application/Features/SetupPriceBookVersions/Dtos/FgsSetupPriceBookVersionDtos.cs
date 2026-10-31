namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;

public sealed record FgsSetupPriceBookVersionSummaryDto(
    long Id,
    string Version,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    bool IsActive);

public sealed record FgsSetupPriceBookVersionDetailDto(
    long Id,
    string Version,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    bool IsActive);

public sealed record FgsSetupPriceBookVersionLookupDto(
    long Id,
    string Version,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo);

public sealed record FgsSetupPriceBookVersionCreateDto(
    string Version,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo = null);

public sealed record FgsSetupPriceBookVersionUpdateDto(
    string Version,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo = null);

public sealed record FgsSetupPriceBookVersionPatchDto(
    string? Version = null,
    DateOnly? EffectiveFrom = null,
    DateOnly? EffectiveTo = null,
    bool? IsActive = null);

public sealed record FgsSetupPriceBookVersionListFilters(
    string? Version = null);
