namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupServiceAssetModelReference</summary>
public sealed record FgsSetupServiceAssetModelReferenceSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupServiceAssetTypeId</summary>
    long FgsSetupServiceAssetTypeId,
    /// <summary>FgsSetupServiceAssetManufacturerId</summary>
    long FgsSetupServiceAssetManufacturerId,
    /// <summary>ModelNumber</summary>
    string? ModelNumber,
    /// <summary>ModelDescription</summary>
    string? ModelDescription,
    /// <summary>SerialNumberPattern</summary>
    string? SerialNumberPattern,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>UrlsJson</summary>
    string? UrlsJson,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupServiceAssetModelReferenceDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupServiceAssetTypeId</summary>
    long FgsSetupServiceAssetTypeId,
    /// <summary>FgsSetupServiceAssetManufacturerId</summary>
    long FgsSetupServiceAssetManufacturerId,
    /// <summary>ModelNumber</summary>
    string? ModelNumber,
    /// <summary>ModelDescription</summary>
    string? ModelDescription,
    /// <summary>SerialNumberPattern</summary>
    string? SerialNumberPattern,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>UrlsJson</summary>
    string? UrlsJson,
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

public sealed record FgsSetupServiceAssetModelReferenceCreateDto(
    /// <summary>FgsSetupServiceAssetTypeId</summary>
    long FgsSetupServiceAssetTypeId,
    /// <summary>FgsSetupServiceAssetManufacturerId</summary>
    long FgsSetupServiceAssetManufacturerId,
    /// <summary>ModelNumber</summary>
    string? ModelNumber,
    /// <summary>ModelDescription</summary>
    string? ModelDescription,
    /// <summary>SerialNumberPattern</summary>
    string? SerialNumberPattern,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>UrlsJson</summary>
    string? UrlsJson)
;

public sealed record FgsSetupServiceAssetModelReferenceUpdateDto(
    /// <summary>FgsSetupServiceAssetTypeId</summary>
    long FgsSetupServiceAssetTypeId,
    /// <summary>FgsSetupServiceAssetManufacturerId</summary>
    long FgsSetupServiceAssetManufacturerId,
    /// <summary>ModelNumber</summary>
    string? ModelNumber,
    /// <summary>ModelDescription</summary>
    string? ModelDescription,
    /// <summary>SerialNumberPattern</summary>
    string? SerialNumberPattern,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>UrlsJson</summary>
    string? UrlsJson)
;

public sealed record FgsSetupServiceAssetModelReferencePatchDto(
    /// <summary>FgsSetupServiceAssetTypeId</summary>
    long? FgsSetupServiceAssetTypeId,
    /// <summary>FgsSetupServiceAssetManufacturerId</summary>
    long? FgsSetupServiceAssetManufacturerId,
    /// <summary>ModelNumber</summary>
    string? ModelNumber,
    /// <summary>ModelDescription</summary>
    string? ModelDescription,
    /// <summary>SerialNumberPattern</summary>
    string? SerialNumberPattern,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>UrlsJson</summary>
    string? UrlsJson)
;

