using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.CreateFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.PatchFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.UpdateFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using Fgs.Inventory.Domain.Entities;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Validators;

public sealed class CreateFgsPurchaseOrderCommandValidator : AbstractValidator<CreateFgsPurchaseOrderCommand>
{
    public CreateFgsPurchaseOrderCommandValidator(
        IFgsPurchaseOrderReadRepository readRepository,
        IFgsVendorReadRepository vendorReadRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.PurchaseOrderNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.PurchaseOrderNumber)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByPurchaseOrderNumberAsync(number, null, cancellationToken))
                .WithMessage("A purchase order with this number already exists.");
            RuleFor(x => x.Dto.VendorId).GreaterThan(0);
            RuleFor(x => x.Dto.VendorId)
                .MustAsync(async (vendorId, cancellationToken) =>
                    await vendorReadRepository.ExistsAsync(vendorId, cancellationToken: cancellationToken))
                .WithMessage("Vendor was not found.");
            RuleFor(x => x.Dto.PurchaseOrderStatus)
                .Must(IsValidPurchaseOrderStatus)
                .WithMessage("PurchaseOrderStatus must be OPEN, PARTIAL, RECEIVED, CLOSED or CANCELLED.");
            RuleFor(x => x.Dto.ShipToInventoryLocationId)
                .MustAsync(async (locationId, cancellationToken) =>
                    await readRepository.ExistsInventoryLocationAsync(locationId!.Value, cancellationToken))
                .WithMessage("Ship-to inventory location was not found.")
                .When(x => x.Dto.ShipToInventoryLocationId.HasValue);

            RuleFor(x => x.Dto.Details)
                .Must(HaveUniqueLineNumbers)
                .WithMessage("Details must not contain duplicate lineNumber values.")
                .When(x => x.Dto.Details is { Count: > 0 });

            RuleForEach(x => x.Dto.Details)
                .SetValidator(new FgsPurchaseOrderLineDtoValidator(readRepository))
                .When(x => x.Dto.Details is not null);
        });
    }

    private static bool IsValidPurchaseOrderStatus(string status) =>
        status is PurchaseOrderStatuses.Open
            or PurchaseOrderStatuses.Partial
            or PurchaseOrderStatuses.Received
            or PurchaseOrderStatuses.Closed
            or PurchaseOrderStatuses.Cancelled;

    private static bool HaveUniqueLineNumbers(IReadOnlyList<FgsPurchaseOrderLineDto>? details) =>
        details is null
        || details.Select(d => d.LineNumber).Distinct().Count() == details.Count;
}

public sealed class UpdateFgsPurchaseOrderCommandValidator : AbstractValidator<UpdateFgsPurchaseOrderCommand>
{
    public UpdateFgsPurchaseOrderCommandValidator(
        IFgsPurchaseOrderReadRepository readRepository,
        IFgsVendorReadRepository vendorReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.PurchaseOrderNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.PurchaseOrderNumber)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByPurchaseOrderNumberAsync(number, command.Id, cancellationToken))
                .WithMessage("A purchase order with this number already exists.");
            RuleFor(x => x.Dto.VendorId).GreaterThan(0);
            RuleFor(x => x.Dto.VendorId)
                .MustAsync(async (vendorId, cancellationToken) =>
                    await vendorReadRepository.ExistsAsync(vendorId, cancellationToken: cancellationToken))
                .WithMessage("Vendor was not found.");
            RuleFor(x => x.Dto.PurchaseOrderStatus)
                .Must(IsValidPurchaseOrderStatus)
                .WithMessage("PurchaseOrderStatus must be OPEN, PARTIAL, RECEIVED, CLOSED or CANCELLED.");
            RuleFor(x => x.Dto.ShipToInventoryLocationId)
                .MustAsync(async (locationId, cancellationToken) =>
                    await readRepository.ExistsInventoryLocationAsync(locationId!.Value, cancellationToken))
                .WithMessage("Ship-to inventory location was not found.")
                .When(x => x.Dto.ShipToInventoryLocationId.HasValue);

            RuleFor(x => x.Dto.Details)
                .Must(HaveUniqueLineNumbers)
                .WithMessage("Details must not contain duplicate lineNumber values.")
                .When(x => x.Dto.Details is { Count: > 0 });

            RuleForEach(x => x.Dto.Details)
                .SetValidator(new FgsPurchaseOrderLineDtoValidator(readRepository))
                .When(x => x.Dto.Details is not null);
        });
    }

    private static bool IsValidPurchaseOrderStatus(string status) =>
        status is PurchaseOrderStatuses.Open
            or PurchaseOrderStatuses.Partial
            or PurchaseOrderStatuses.Received
            or PurchaseOrderStatuses.Closed
            or PurchaseOrderStatuses.Cancelled;

    private static bool HaveUniqueLineNumbers(IReadOnlyList<FgsPurchaseOrderLineDto>? details) =>
        details is null
        || details.Select(d => d.LineNumber).Distinct().Count() == details.Count;
}

public sealed class PatchFgsPurchaseOrderCommandValidator : AbstractValidator<PatchFgsPurchaseOrderCommand>
{
    public PatchFgsPurchaseOrderCommandValidator(
        IFgsPurchaseOrderReadRepository readRepository,
        IFgsVendorReadRepository vendorReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.PurchaseOrderNumber).NotEmpty().MaximumLength(50)
                .When(x => x.Dto.PurchaseOrderNumber is not null);
            RuleFor(x => x.Dto.PurchaseOrderNumber!)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByPurchaseOrderNumberAsync(number, command.Id, cancellationToken))
                .WithMessage("A purchase order with this number already exists.")
                .When(x => x.Dto.PurchaseOrderNumber is not null);
            RuleFor(x => x.Dto.VendorId!.Value).GreaterThan(0)
                .When(x => x.Dto.VendorId.HasValue);
            RuleFor(x => x.Dto.VendorId!.Value)
                .MustAsync(async (vendorId, cancellationToken) =>
                    await vendorReadRepository.ExistsAsync(vendorId, cancellationToken: cancellationToken))
                .WithMessage("Vendor was not found.")
                .When(x => x.Dto.VendorId.HasValue);
            RuleFor(x => x.Dto.PurchaseOrderStatus!)
                .Must(IsValidPurchaseOrderStatus)
                .WithMessage("PurchaseOrderStatus must be OPEN, PARTIAL, RECEIVED, CLOSED or CANCELLED.")
                .When(x => x.Dto.PurchaseOrderStatus is not null);
            RuleFor(x => x.Dto.ShipToInventoryLocationId!.Value)
                .MustAsync(async (locationId, cancellationToken) =>
                    await readRepository.ExistsInventoryLocationAsync(locationId, cancellationToken))
                .WithMessage("Ship-to inventory location was not found.")
                .When(x => x.Dto.ShipToInventoryLocationId.HasValue);

            RuleFor(x => x.Dto.Details)
                .Must(HaveUniqueLineNumbers)
                .WithMessage("Details must not contain duplicate lineNumber values.")
                .When(x => x.Dto.Details is { Count: > 0 });

            RuleForEach(x => x.Dto.Details)
                .SetValidator(new FgsPurchaseOrderLineDtoValidator(readRepository))
                .When(x => x.Dto.Details is not null);
        });
    }

    private static bool IsValidPurchaseOrderStatus(string status) =>
        status is PurchaseOrderStatuses.Open
            or PurchaseOrderStatuses.Partial
            or PurchaseOrderStatuses.Received
            or PurchaseOrderStatuses.Closed
            or PurchaseOrderStatuses.Cancelled;

    private static bool HaveUniqueLineNumbers(IReadOnlyList<FgsPurchaseOrderLineDto>? details) =>
        details is null
        || details.Select(d => d.LineNumber).Distinct().Count() == details.Count;
}

internal sealed class FgsPurchaseOrderLineDtoValidator : AbstractValidator<FgsPurchaseOrderLineDto>
{
    public FgsPurchaseOrderLineDtoValidator(IFgsPurchaseOrderReadRepository readRepository)
    {
        RuleFor(x => x.Id!.Value).GreaterThan(0)
            .When(x => x.Id.HasValue);
        RuleFor(x => x.LineNumber).GreaterThan((short)0);
        RuleFor(x => x.ItemId).GreaterThan(0);
        RuleFor(x => x.ItemId)
            .MustAsync(async (itemId, cancellationToken) =>
                await readRepository.ExistsInventoryItemAsync(itemId, cancellationToken))
            .WithMessage("Inventory item was not found.");
        RuleFor(x => x.ItemDescription).NotEmpty().MaximumLength(255);
        RuleFor(x => x.UnitOfMeasureCode).NotEmpty().MaximumLength(25);
        RuleFor(x => x.OrderedQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ReceivedQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.VendorPartNumber).MaximumLength(100)
            .When(x => x.VendorPartNumber is not null);
    }
}
