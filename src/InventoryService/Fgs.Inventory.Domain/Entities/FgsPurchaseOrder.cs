using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Purchase order header for inventory purchased from vendors.
/// </summary>
public class FgsPurchaseOrder : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string PurchaseOrderNumber { get; set; } = null!;

    public long VendorId { get; set; }

    public string PurchaseOrderStatus { get; set; } = PurchaseOrderStatuses.Open;

    public DateTimeOffset PurchaseOrderDate { get; set; }

    public DateTimeOffset? ExpectedDeliveryDate { get; set; }

    /// <summary>References setup.FgsEmployee; scalar only — no cross-schema FK.</summary>
    public long? RequestedByEmployeeId { get; set; }

    public string? RequestedByName { get; set; }

    /// <summary>References setup.FgsEmployee; scalar only — no cross-schema FK.</summary>
    public long? BuyerEmployeeId { get; set; }

    public long? ShipToInventoryLocationId { get; set; }

    /// <summary>References setup/dispatch service location; scalar only — no cross-schema FK.</summary>
    public long? ShipToServiceLocationId { get; set; }

    public string? ShipToName { get; set; }

    public string? ShipToAddress1 { get; set; }

    public string? ShipToAddress2 { get; set; }

    public string? ShipToCity { get; set; }

    public string? ShipToStateProvince { get; set; }

    public string? ShipToPostalCode { get; set; }

    public string? ShipToCountry { get; set; }

    public string? VendorReferenceNumber { get; set; }

    public string? VendorContactName { get; set; }

    public string? VendorEmail { get; set; }

    public string? VendorPhoneNumber { get; set; }

    public decimal Subtotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxableAmount { get; set; }

    public string? PurchaseTaxJson { get; set; }

    public decimal FreightAmount { get; set; }

    public decimal OtherCharges { get; set; }

    public decimal TotalAmount { get; set; }

    public string? VendorNotes { get; set; }

    public string? InternalNotes { get; set; }
}
