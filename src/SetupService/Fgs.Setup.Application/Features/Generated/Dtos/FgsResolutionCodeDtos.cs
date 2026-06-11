namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsResolutionCode</summary>
public sealed record FgsResolutionCodeSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>GloResolutionTypeId</summary>
    int GloResolutionTypeId,
    /// <summary>ResolutionCode</summary>
    string? ResolutionCode,
    /// <summary>ResolutionName</summary>
    string? ResolutionName,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsResolutionCodeDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>GloResolutionTypeId</summary>
    int GloResolutionTypeId,
    /// <summary>ResolutionCode</summary>
    string? ResolutionCode,
    /// <summary>ResolutionName</summary>
    string? ResolutionName,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
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

public sealed record FgsResolutionCodeCreateDto(
    /// <summary>GloResolutionTypeId</summary>
    int GloResolutionTypeId,
    /// <summary>ResolutionCode</summary>
    string? ResolutionCode,
    /// <summary>ResolutionName</summary>
    string? ResolutionName,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible)
;

public sealed record FgsResolutionCodeUpdateDto(
    /// <summary>GloResolutionTypeId</summary>
    int GloResolutionTypeId,
    /// <summary>ResolutionCode</summary>
    string? ResolutionCode,
    /// <summary>ResolutionName</summary>
    string? ResolutionName,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible)
;

public sealed record FgsResolutionCodePatchDto(
    /// <summary>GloResolutionTypeId</summary>
    int? GloResolutionTypeId,
    /// <summary>ResolutionCode</summary>
    string? ResolutionCode,
    /// <summary>ResolutionName</summary>
    string? ResolutionName,
    /// <summary>IsMobileVisible</summary>
    bool? IsMobileVisible)
;

