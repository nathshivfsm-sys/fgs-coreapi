namespace Fgs.Setup.Application.Features.SetupDescriptions.Dtos;

public sealed record FgsSetupDescriptionSummaryDto(
    long Id,
    string DescriptionTypeCode,
    string? ShortNote,
    string Body,
    long? FgsSetupTechTradeId,
    int SortOrder,
    bool IsActive);

public sealed record FgsSetupDescriptionDetailDto(
    long Id,
    string DescriptionTypeCode,
    string? ShortNote,
    string Body,
    long? FgsSetupTechTradeId,
    int SortOrder,
    bool IsActive);

public sealed record FgsSetupDescriptionLookupDto(
    long Id,
    string DescriptionTypeCode,
    string Body,
    int SortOrder);

public sealed record FgsSetupDescriptionCreateDto(
    string DescriptionTypeCode,
    string? ShortNote,
    string Body,
    long? FgsSetupTechTradeId,
    int SortOrder);

public sealed record FgsSetupDescriptionUpdateDto(
    string DescriptionTypeCode,
    string? ShortNote,
    string Body,
    long? FgsSetupTechTradeId,
    int SortOrder);

public sealed record FgsSetupDescriptionPatchDto(
    string? DescriptionTypeCode,
    string? ShortNote,
    string? Body,
    long? FgsSetupTechTradeId,
    int? SortOrder,
    bool? IsActive);

public sealed record FgsSetupDescriptionListFilters(
    string? DescriptionTypeCode = null);
