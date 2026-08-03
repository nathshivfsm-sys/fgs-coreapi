using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;

namespace Fgs.Inventory.Infrastructure.PurchaseOrders;

internal sealed class FgsPurchaseOrderSummaryRow
{
    public long Id { get; set; }
    public string PurchaseOrderNumber { get; set; } = null!;
    public long VendorId { get; set; }
    public string PurchaseOrderStatus { get; set; } = null!;
    public DateTimeOffset PurchaseOrderDate { get; set; }
    public DateTimeOffset? ExpectedDeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }

    public FgsPurchaseOrderSummaryDto ToDto() =>
        new(Id, PurchaseOrderNumber, VendorId, PurchaseOrderStatus, PurchaseOrderDate, ExpectedDeliveryDate, TotalAmount);
}

internal sealed class FgsPurchaseOrderDetailRow
{
    public long Id { get; set; }
    public string PurchaseOrderNumber { get; set; } = null!;
    public long VendorId { get; set; }
    public string PurchaseOrderStatus { get; set; } = null!;
    public DateTimeOffset PurchaseOrderDate { get; set; }
    public DateTimeOffset? ExpectedDeliveryDate { get; set; }
    public long? RequestedByEmployeeId { get; set; }
    public string? RequestedByName { get; set; }
    public long? BuyerEmployeeId { get; set; }
    public long? ShipToInventoryLocationId { get; set; }
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

    public FgsPurchaseOrderDetailDto ToDto(IReadOnlyList<FgsPurchaseOrderLineDetailDto> details) =>
        new(
            Id,
            PurchaseOrderNumber,
            VendorId,
            PurchaseOrderStatus,
            PurchaseOrderDate,
            ExpectedDeliveryDate,
            RequestedByEmployeeId,
            RequestedByName,
            BuyerEmployeeId,
            ShipToInventoryLocationId,
            ShipToServiceLocationId,
            ShipToName,
            ShipToAddress1,
            ShipToAddress2,
            ShipToCity,
            ShipToStateProvince,
            ShipToPostalCode,
            ShipToCountry,
            VendorReferenceNumber,
            VendorContactName,
            VendorEmail,
            VendorPhoneNumber,
            Subtotal,
            DiscountAmount,
            TaxableAmount,
            PurchaseTaxJson,
            FreightAmount,
            OtherCharges,
            TotalAmount,
            VendorNotes,
            InternalNotes,
            details);
}

internal sealed class FgsPurchaseOrderLineRow
{
    public long Id { get; set; }
    public short LineNumber { get; set; }
    public long ItemId { get; set; }
    public string? VendorPartNumber { get; set; }
    public string ItemDescription { get; set; } = null!;
    public string UnitOfMeasureCode { get; set; } = null!;
    public decimal OrderedQuantity { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal DiscountAmount { get; set; }
    public bool IsTaxable { get; set; }
    public decimal ExtendedAmount { get; set; }
    public DateTimeOffset? ExpectedDeliveryDate { get; set; }
    public string? Notes { get; set; }

    public FgsPurchaseOrderLineDetailDto ToDto() =>
        new(
            Id,
            LineNumber,
            ItemId,
            VendorPartNumber,
            ItemDescription,
            UnitOfMeasureCode,
            OrderedQuantity,
            ReceivedQuantity,
            UnitCost,
            DiscountAmount,
            IsTaxable,
            ExtendedAmount,
            ExpectedDeliveryDate,
            Notes);
}
