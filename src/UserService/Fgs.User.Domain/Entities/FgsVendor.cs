namespace Fgs.User.Domain.Entities;

/// <summary>
/// Vendor and subcontractor master record for purchasing, AP, and 1099 reporting.
/// </summary>
public class FgsVendor : FgsTenantCompanySetupEntityBase<long>
{
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
}
