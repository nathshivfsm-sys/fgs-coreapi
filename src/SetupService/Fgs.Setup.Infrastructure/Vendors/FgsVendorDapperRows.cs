using Fgs.Setup.Application.Features.Vendors.Dtos;

namespace Fgs.Setup.Infrastructure.Vendors;

internal sealed class FgsVendorSummaryRow
{
    public long Id { get; set; }
    public string VendorCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? LegalName { get; set; }
    public string VendorType { get; set; } = null!;
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

    public FgsVendorSummaryDto ToDto() =>
        new(
            Id,
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
            IsActive);
}

internal sealed class FgsVendorDetailRow
{
    public long Id { get; set; }
    public string VendorCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? LegalName { get; set; }
    public string VendorType { get; set; } = null!;
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

    public FgsVendorDetailDto ToDto() =>
        new(
            Id,
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
            IsActive);
}

internal sealed class FgsVendorLookupRow
{
    public long Id { get; set; }
    public string VendorCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsVendorLookupDto ToDto() => new(Id,
            VendorCode,
            Name);
}
