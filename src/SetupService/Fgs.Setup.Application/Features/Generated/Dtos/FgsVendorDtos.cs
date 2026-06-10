namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsVendor</summary>
public sealed record FgsVendorSummaryDto(
    /// <summary>Allowed values: VENDOR, SUBCONTRACTOR</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>VendorCode</summary>
    string? VendorCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>LegalName</summary>
    string? LegalName,
    /// <summary>VendorType</summary>
    string? VendorType,
    /// <summary>PaymentTermId</summary>
    long? PaymentTermId,
    /// <summary>Indicates whether vendor should be included in 1099 reporting.</summary>
    string? Email,
    /// <summary>PhoneNumber</summary>
    string? PhoneNumber,
    /// <summary>MobileNumber</summary>
    string? MobileNumber,
    /// <summary>Website</summary>
    string? Website,
    /// <summary>TaxIdentificationNumber</summary>
    string? TaxIdentificationNumber,
    /// <summary>LicenseNumber</summary>
    string? LicenseNumber,
    /// <summary>InsurancePolicyNumber</summary>
    string? InsurancePolicyNumber,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>Is1099Eligible</summary>
    bool Is1099Eligible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>References payment terms used for accounts payable due date calculation.</summary>
    bool IsActive)
;

public sealed record FgsVendorDetailDto(
    /// <summary>Allowed values: VENDOR, SUBCONTRACTOR</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>VendorCode</summary>
    string? VendorCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>LegalName</summary>
    string? LegalName,
    /// <summary>VendorType</summary>
    string? VendorType,
    /// <summary>PaymentTermId</summary>
    long? PaymentTermId,
    /// <summary>Indicates whether vendor should be included in 1099 reporting.</summary>
    string? Email,
    /// <summary>PhoneNumber</summary>
    string? PhoneNumber,
    /// <summary>MobileNumber</summary>
    string? MobileNumber,
    /// <summary>Website</summary>
    string? Website,
    /// <summary>TaxIdentificationNumber</summary>
    string? TaxIdentificationNumber,
    /// <summary>LicenseNumber</summary>
    string? LicenseNumber,
    /// <summary>InsurancePolicyNumber</summary>
    string? InsurancePolicyNumber,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>Is1099Eligible</summary>
    bool Is1099Eligible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>References payment terms used for accounts payable due date calculation.</summary>
    bool IsActive)
;

public sealed record FgsVendorCreateDto(
    /// <summary>VendorCode</summary>
    string? VendorCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>LegalName</summary>
    string? LegalName,
    /// <summary>VendorType</summary>
    string? VendorType,
    /// <summary>PaymentTermId</summary>
    long? PaymentTermId,
    /// <summary>Indicates whether vendor should be included in 1099 reporting.</summary>
    string? Email,
    /// <summary>PhoneNumber</summary>
    string? PhoneNumber,
    /// <summary>MobileNumber</summary>
    string? MobileNumber,
    /// <summary>Website</summary>
    string? Website,
    /// <summary>TaxIdentificationNumber</summary>
    string? TaxIdentificationNumber,
    /// <summary>LicenseNumber</summary>
    string? LicenseNumber,
    /// <summary>InsurancePolicyNumber</summary>
    string? InsurancePolicyNumber,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>Is1099Eligible</summary>
    bool Is1099Eligible)
;

public sealed record FgsVendorUpdateDto(
    /// <summary>VendorCode</summary>
    string? VendorCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>LegalName</summary>
    string? LegalName,
    /// <summary>VendorType</summary>
    string? VendorType,
    /// <summary>PaymentTermId</summary>
    long? PaymentTermId,
    /// <summary>Indicates whether vendor should be included in 1099 reporting.</summary>
    string? Email,
    /// <summary>PhoneNumber</summary>
    string? PhoneNumber,
    /// <summary>MobileNumber</summary>
    string? MobileNumber,
    /// <summary>Website</summary>
    string? Website,
    /// <summary>TaxIdentificationNumber</summary>
    string? TaxIdentificationNumber,
    /// <summary>LicenseNumber</summary>
    string? LicenseNumber,
    /// <summary>InsurancePolicyNumber</summary>
    string? InsurancePolicyNumber,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>Is1099Eligible</summary>
    bool Is1099Eligible)
;

public sealed record FgsVendorPatchDto(
    /// <summary>VendorCode</summary>
    string? VendorCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>LegalName</summary>
    string? LegalName,
    /// <summary>VendorType</summary>
    string? VendorType,
    /// <summary>PaymentTermId</summary>
    long? PaymentTermId,
    /// <summary>Indicates whether vendor should be included in 1099 reporting.</summary>
    string? Email,
    /// <summary>PhoneNumber</summary>
    string? PhoneNumber,
    /// <summary>MobileNumber</summary>
    string? MobileNumber,
    /// <summary>Website</summary>
    string? Website,
    /// <summary>TaxIdentificationNumber</summary>
    string? TaxIdentificationNumber,
    /// <summary>LicenseNumber</summary>
    string? LicenseNumber,
    /// <summary>InsurancePolicyNumber</summary>
    string? InsurancePolicyNumber,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>Is1099Eligible</summary>
    bool? Is1099Eligible)
;

