using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.CreateFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.PatchFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using Fgs.Inventory.Application.Features.PurchaseOrders.Validators;
using Fgs.Inventory.Domain.Entities;
using Moq;

namespace Fgs.Inventory.Tests.PurchaseOrders;

public sealed class FgsPurchaseOrderValidatorTests
{
    private readonly Mock<IFgsPurchaseOrderReadRepository> _readRepository = new();
    private readonly Mock<IFgsVendorReadRepository> _vendorReadRepository = new();

    [Fact]
    public async Task CreateValidator_WhenPurchaseOrderNumberMissing_HasValidationError()
    {
        var validator = new CreateFgsPurchaseOrderCommandValidator(
            _readRepository.Object,
            _vendorReadRepository.Object);
        var command = new CreateFgsPurchaseOrderCommand(
            SampleCreateDto(purchaseOrderNumber: ""));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.PurchaseOrderNumber");
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateLineNumbers_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsByPurchaseOrderNumberAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _vendorReadRepository
            .Setup(r => r.ExistsAsync(1, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsInventoryItemAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsPurchaseOrderCommandValidator(
            _readRepository.Object,
            _vendorReadRepository.Object);
        var command = new CreateFgsPurchaseOrderCommand(
            SampleCreateDto(details:
            [
                new FgsPurchaseOrderLineDto(null, 1, 10, null, "Item A", "EA", 1m, 0m, 5m, 0m, true, 5m, null, null),
                new FgsPurchaseOrderLineDto(null, 1, 10, null, "Item B", "EA", 2m, 0m, 5m, 0m, true, 10m, null, null)
            ]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("lineNumber"));
    }

    [Fact]
    public async Task PatchValidator_WhenCancelStatus_IsValid()
    {
        _readRepository
            .Setup(r => r.ExistsByPurchaseOrderNumberAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new PatchFgsPurchaseOrderCommandValidator(
            _readRepository.Object,
            _vendorReadRepository.Object);
        var command = new PatchFgsPurchaseOrderCommand(
            5,
            new FgsPurchaseOrderPatchDto(
                PurchaseOrderNumber: null,
                VendorId: null,
                PurchaseOrderStatus: PurchaseOrderStatuses.Cancelled,
                PurchaseOrderDate: null,
                ExpectedDeliveryDate: null,
                RequestedByEmployeeId: null,
                RequestedByName: null,
                BuyerEmployeeId: null,
                ShipToInventoryLocationId: null,
                ShipToServiceLocationId: null,
                ShipToName: null,
                ShipToAddress1: null,
                ShipToAddress2: null,
                ShipToCity: null,
                ShipToStateProvince: null,
                ShipToPostalCode: null,
                ShipToCountry: null,
                VendorReferenceNumber: null,
                VendorContactName: null,
                VendorEmail: null,
                VendorPhoneNumber: null,
                Subtotal: null,
                DiscountAmount: null,
                TaxableAmount: null,
                PurchaseTaxJson: null,
                FreightAmount: null,
                OtherCharges: null,
                TotalAmount: null,
                VendorNotes: null,
                InternalNotes: null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    private static FgsPurchaseOrderCreateDto SampleCreateDto(
        string purchaseOrderNumber = "PO-1001",
        IReadOnlyList<FgsPurchaseOrderLineDto>? details = null) =>
        new(
            purchaseOrderNumber,
            VendorId: 1,
            PurchaseOrderStatuses.Open,
            PurchaseOrderDate: DateTimeOffset.UtcNow,
            ExpectedDeliveryDate: null,
            RequestedByEmployeeId: null,
            RequestedByName: null,
            BuyerEmployeeId: null,
            ShipToInventoryLocationId: null,
            ShipToServiceLocationId: null,
            ShipToName: null,
            ShipToAddress1: null,
            ShipToAddress2: null,
            ShipToCity: null,
            ShipToStateProvince: null,
            ShipToPostalCode: null,
            ShipToCountry: null,
            VendorReferenceNumber: null,
            VendorContactName: null,
            VendorEmail: null,
            VendorPhoneNumber: null,
            Subtotal: 0m,
            DiscountAmount: 0m,
            TaxableAmount: 0m,
            PurchaseTaxJson: null,
            FreightAmount: 0m,
            OtherCharges: 0m,
            TotalAmount: 0m,
            VendorNotes: null,
            InternalNotes: null,
            details);
}
