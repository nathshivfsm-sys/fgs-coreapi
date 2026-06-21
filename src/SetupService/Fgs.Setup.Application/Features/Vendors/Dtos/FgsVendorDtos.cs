namespace Fgs.Setup.Application.Features.Vendors.Dtos;

public sealed record FgsVendorSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string VendorCode,
    string Name,
    string? LegalName,
    string VendorType,
    long? PaymentTermId,
    string? Email,
    string? PhoneNumber,
    string? MobileNumber,
    string? Website,
    string? TaxIdentificationNumber,
    string? LicenseNumber,
    string? InsurancePolicyNumber,
    string? Notes,
    bool Is1099Eligible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsVendorDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string VendorCode,
    string Name,
    string? LegalName,
    string VendorType,
    long? PaymentTermId,
    string? Email,
    string? PhoneNumber,
    string? MobileNumber,
    string? Website,
    string? TaxIdentificationNumber,
    string? LicenseNumber,
    string? InsurancePolicyNumber,
    string? Notes,
    bool Is1099Eligible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsVendorLookupDto(
    long Id,
    string VendorCode,
    string Name);

public sealed record FgsVendorCreateDto(
    string VendorCode,
    string Name,
    string? LegalName,
    string VendorType,
    long? PaymentTermId,
    string? Email,
    string? PhoneNumber,
    string? MobileNumber,
    string? Website,
    string? TaxIdentificationNumber,
    string? LicenseNumber,
    string? InsurancePolicyNumber,
    string? Notes,
    bool Is1099Eligible);

public sealed record FgsVendorUpdateDto(
    string VendorCode,
    string Name,
    string? LegalName,
    string VendorType,
    long? PaymentTermId,
    string? Email,
    string? PhoneNumber,
    string? MobileNumber,
    string? Website,
    string? TaxIdentificationNumber,
    string? LicenseNumber,
    string? InsurancePolicyNumber,
    string? Notes,
    bool Is1099Eligible);

public sealed record FgsVendorPatchDto(
    string? VendorCode,
    string? Name,
    string? LegalName,
    string? VendorType,
    long? PaymentTermId,
    string? Email,
    string? PhoneNumber,
    string? MobileNumber,
    string? Website,
    string? TaxIdentificationNumber,
    string? LicenseNumber,
    string? InsurancePolicyNumber,
    string? Notes,
    bool? Is1099Eligible,
    bool? IsActive);

public sealed record FgsVendorListFilters(
    string? VendorCode = null,
    string? Name = null);
