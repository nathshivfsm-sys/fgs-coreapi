namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupDescription</summary>
public sealed record FgsSetupDescriptionSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>DescriptionTypeCode</summary>
    string? DescriptionTypeCode,
    /// <summary>ShortNote</summary>
    string? ShortNote,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>FgsSetupTechTradeId</summary>
    long? FgsSetupTechTradeId,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupDescriptionDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>DescriptionTypeCode</summary>
    string? DescriptionTypeCode,
    /// <summary>ShortNote</summary>
    string? ShortNote,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>FgsSetupTechTradeId</summary>
    long? FgsSetupTechTradeId,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupDescriptionCreateDto(
    /// <summary>DescriptionTypeCode</summary>
    string? DescriptionTypeCode,
    /// <summary>ShortNote</summary>
    string? ShortNote,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>FgsSetupTechTradeId</summary>
    long? FgsSetupTechTradeId,
    /// <summary>SortOrder</summary>
    int SortOrder)
;

public sealed record FgsSetupDescriptionUpdateDto(
    /// <summary>DescriptionTypeCode</summary>
    string? DescriptionTypeCode,
    /// <summary>ShortNote</summary>
    string? ShortNote,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>FgsSetupTechTradeId</summary>
    long? FgsSetupTechTradeId,
    /// <summary>SortOrder</summary>
    int SortOrder)
;

public sealed record FgsSetupDescriptionPatchDto(
    /// <summary>DescriptionTypeCode</summary>
    string? DescriptionTypeCode,
    /// <summary>ShortNote</summary>
    string? ShortNote,
    /// <summary>Body</summary>
    string? Body,
    /// <summary>FgsSetupTechTradeId</summary>
    long? FgsSetupTechTradeId,
    /// <summary>SortOrder</summary>
    int? SortOrder)
;

