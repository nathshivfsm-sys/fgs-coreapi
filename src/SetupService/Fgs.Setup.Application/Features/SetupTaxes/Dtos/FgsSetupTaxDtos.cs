namespace Fgs.Setup.Application.Features.SetupTaxes.Dtos;

public sealed record FgsSetupTaxAuthorityAssignmentWriteDto(
    long FgsSetupTaxAuthorityId,
    DateOnly EffectiveFromDate,
    DateOnly? EffectiveToDate,
    bool IsExternalSystemRecord);

public sealed record FgsSetupTaxAuthorityAssignmentDto(
    long Id,
    long FgsSetupTaxAuthorityId,
    string TaxAuthorityCode,
    string TaxAuthorityName,
    decimal TaxPercent,
    DateOnly EffectiveFromDate,
    DateOnly? EffectiveToDate,
    bool IsExternalSystemRecord,
    bool IsActive);

public sealed record FgsSetupTaxSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string TaxCode,
    string Name,
    bool IsExternalSystemRecord,
    string? ExternalSystemId,
    string? SyncToken,
    bool ShowTaxDetail,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSetupTaxDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string TaxCode,
    string Name,
    bool IsExternalSystemRecord,
    string? ExternalSystemId,
    string? SyncToken,
    bool ShowTaxDetail,
    string? Description,
    IReadOnlyList<FgsSetupTaxAuthorityAssignmentDto> TaxDetails,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSetupTaxLookupDto(
    long Id,
    string TaxCode,
    string Name);

public sealed record FgsSetupTaxCreateDto(
    string TaxCode,
    string Name,
    bool IsExternalSystemRecord,
    string? ExternalSystemId,
    string? SyncToken,
    bool ShowTaxDetail,
    string? Description,
    IReadOnlyList<FgsSetupTaxAuthorityAssignmentWriteDto>? TaxDetails = null);

public sealed record FgsSetupTaxUpdateDto(
    string TaxCode,
    string Name,
    bool IsExternalSystemRecord,
    string? ExternalSystemId,
    string? SyncToken,
    bool ShowTaxDetail,
    string? Description,
    IReadOnlyList<FgsSetupTaxAuthorityAssignmentWriteDto>? TaxDetails = null);

public sealed record FgsSetupTaxPatchDto(
    string? TaxCode,
    string? Name,
    bool? IsExternalSystemRecord,
    string? ExternalSystemId,
    string? SyncToken,
    bool? ShowTaxDetail,
    string? Description,
    IReadOnlyList<FgsSetupTaxAuthorityAssignmentWriteDto>? TaxDetails,
    bool? IsActive);

public sealed record FgsSetupTaxListFilters(
    string? TaxCode = null,
    string? Name = null);
