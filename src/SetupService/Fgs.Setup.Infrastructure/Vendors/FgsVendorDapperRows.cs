using Fgs.Setup.Application.Features.Vendors.Dtos;

namespace Fgs.Setup.Infrastructure.Vendors;

internal sealed class FgsVendorSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string VendorCode { get; set; }
    public string Name { get; set; }
    public string? LegalName { get; set; }
    public string VendorType { get; set; }
    public long? PaymentTermId { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? Website { get; set; }
    public string? TaxIdentificationNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? InsurancePolicyNumber { get; set; }
    public string? Notes { get; set; }
    public bool Is1099Eligible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsVendorSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            VendorCode,
            Name,
            LegalName,
            VendorType,
            PaymentTermId,
            Email,
            PhoneNumber,
            MobileNumber,
            Website,
            TaxIdentificationNumber,
            LicenseNumber,
            InsurancePolicyNumber,
            Notes,
            Is1099Eligible,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsVendorDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string VendorCode { get; set; }
    public string Name { get; set; }
    public string? LegalName { get; set; }
    public string VendorType { get; set; }
    public long? PaymentTermId { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? Website { get; set; }
    public string? TaxIdentificationNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? InsurancePolicyNumber { get; set; }
    public string? Notes { get; set; }
    public bool Is1099Eligible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsVendorDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            VendorCode,
            Name,
            LegalName,
            VendorType,
            PaymentTermId,
            Email,
            PhoneNumber,
            MobileNumber,
            Website,
            TaxIdentificationNumber,
            LicenseNumber,
            InsurancePolicyNumber,
            Notes,
            Is1099Eligible,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsVendorLookupRow
{
    public long Id { get; set; }
    public string VendorCode { get; set; }
    public string Name { get; set; }

    public FgsVendorLookupDto ToDto() => new(Id,
            VendorCode,
            Name);
}
