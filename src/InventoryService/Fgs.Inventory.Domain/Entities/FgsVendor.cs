using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Vendor and subcontractor master record for purchasing, AP, and 1099 reporting.
/// </summary>
public class FgsVendor : FgsTenantCompanySetupEntityBase<long>
{
    public string VendorCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? LegalName { get; set; }

    public string VendorType { get; set; } = null!;

    public string VendorStatus { get; set; } = VendorStatuses.Active;

    public string? VendorAccountNumber { get; set; }

    /// <summary>References setup.FgsSetupPaymentTerm; scalar only — no cross-schema FK.</summary>
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
}
