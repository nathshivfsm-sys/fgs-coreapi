namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupTax</summary>
public sealed record FgsSetupTaxSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>TaxCode</summary>
    string? TaxCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
    /// <summary>ExternalSystemId</summary>
    string? ExternalSystemId,
    /// <summary>SyncToken</summary>
    string? SyncToken,
    /// <summary>ShowTaxDetail</summary>
    bool ShowTaxDetail,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupTaxDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>TaxCode</summary>
    string? TaxCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
    /// <summary>ExternalSystemId</summary>
    string? ExternalSystemId,
    /// <summary>SyncToken</summary>
    string? SyncToken,
    /// <summary>ShowTaxDetail</summary>
    bool ShowTaxDetail,
    /// <summary>Description</summary>
    string? Description,
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

public sealed record FgsSetupTaxCreateDto(
    /// <summary>TaxCode</summary>
    string? TaxCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
    /// <summary>ExternalSystemId</summary>
    string? ExternalSystemId,
    /// <summary>SyncToken</summary>
    string? SyncToken,
    /// <summary>ShowTaxDetail</summary>
    bool ShowTaxDetail,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsSetupTaxUpdateDto(
    /// <summary>TaxCode</summary>
    string? TaxCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
    /// <summary>ExternalSystemId</summary>
    string? ExternalSystemId,
    /// <summary>SyncToken</summary>
    string? SyncToken,
    /// <summary>ShowTaxDetail</summary>
    bool ShowTaxDetail,
    /// <summary>Description</summary>
    string? Description)
;

public sealed record FgsSetupTaxPatchDto(
    /// <summary>TaxCode</summary>
    string? TaxCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>IsExternalSystemRecord</summary>
    bool? IsExternalSystemRecord,
    /// <summary>ExternalSystemId</summary>
    string? ExternalSystemId,
    /// <summary>SyncToken</summary>
    string? SyncToken,
    /// <summary>ShowTaxDetail</summary>
    bool? ShowTaxDetail,
    /// <summary>Description</summary>
    string? Description)
;

