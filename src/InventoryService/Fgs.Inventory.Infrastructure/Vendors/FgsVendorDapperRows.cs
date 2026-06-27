using Fgs.Inventory.Application.Features.Vendors.Dtos;

namespace Fgs.Inventory.Infrastructure.Vendors;

internal sealed class FgsVendorSummaryRow
{
    public long Id { get; set; }
    public string VendorCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? LegalName { get; set; }
    public string VendorType { get; set; } = null!;
    public string VendorStatus { get; set; } = null!;
    public string? VendorAccountNumber { get; set; }
    public long? PaymentTermId { get; set; }
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Email { get; set; }
    public string? PurchaseOrderEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string? Website { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? StateProvince { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? TaxIdNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? InsurancePolicyNumber { get; set; }
    public string? Notes { get; set; }
    public bool Is1099Eligible { get; set; }
    public bool IsActive { get; set; }

    public FgsVendorSummaryDto ToDto() =>
        new(
            Id,
            VendorCode,
            Name,
            LegalName,
            VendorType,
            VendorStatus,
            VendorAccountNumber,
            PaymentTermId,
            ContactName,
            ContactTitle,
            Email,
            PurchaseOrderEmail,
            PhoneNumber,
            MobileNumber,
            FaxNumber,
            Website,
            Address1,
            Address2,
            City,
            StateProvince,
            PostalCode,
            Country,
            TaxIdNumber,
            LicenseNumber,
            InsurancePolicyNumber,
            Notes,
            Is1099Eligible,
            IsActive);
}

internal sealed class FgsVendorDetailRow
{
    public long Id { get; set; }
    public string VendorCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? LegalName { get; set; }
    public string VendorType { get; set; } = null!;
    public string VendorStatus { get; set; } = null!;
    public string? VendorAccountNumber { get; set; }
    public long? PaymentTermId { get; set; }
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Email { get; set; }
    public string? PurchaseOrderEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string? Website { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? StateProvince { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? TaxIdNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? InsurancePolicyNumber { get; set; }
    public string? Notes { get; set; }
    public bool Is1099Eligible { get; set; }
    public bool IsActive { get; set; }

    public FgsVendorDetailDto ToDto() =>
        new(
            Id,
            VendorCode,
            Name,
            LegalName,
            VendorType,
            VendorStatus,
            VendorAccountNumber,
            PaymentTermId,
            ContactName,
            ContactTitle,
            Email,
            PurchaseOrderEmail,
            PhoneNumber,
            MobileNumber,
            FaxNumber,
            Website,
            Address1,
            Address2,
            City,
            StateProvince,
            PostalCode,
            Country,
            TaxIdNumber,
            LicenseNumber,
            InsurancePolicyNumber,
            Notes,
            Is1099Eligible,
            IsActive);
}

internal sealed class FgsVendorLookupRow
{
    public long Id { get; set; }
    public string VendorCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsVendorLookupDto ToDto() => new(Id, VendorCode, Name);
}
