using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.CreateFgsVendorInventoryItem;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.PatchFgsVendorInventoryItem;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.UpdateFgsVendorInventoryItem;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Validators;

public sealed class CreateFgsVendorInventoryItemCommandValidator : AbstractValidator<CreateFgsVendorInventoryItemCommand>
{
    public CreateFgsVendorInventoryItemCommandValidator(
        IFgsVendorReadRepository vendorReadRepository,
        IFgsVendorInventoryItemReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables like {{vendorId}} or {{inventoryItemId}} produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.VendorId).GreaterThan(0);
            RuleFor(x => x.Dto.VendorId)
                .MustAsync(async (vendorId, cancellationToken) =>
                    await vendorReadRepository.ExistsAsync(vendorId, cancellationToken: cancellationToken))
                .WithMessage("Vendor does not exist.");
            RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId, cancellationToken))
                .WithMessage("Inventory item does not exist.");
            RuleFor(x => x.Dto)
                .MustAsync(async (dto, cancellationToken) =>
                    !await readRepository.ExistsByVendorAndItemAsync(dto.VendorId, dto.InventoryItemId, null, cancellationToken))
                .WithMessage("A vendor inventory item for this vendor and item already exists.");
            RuleFor(x => x.Dto.VendorPartNumber).MaximumLength(100);
            RuleFor(x => x.Dto.VendorPartName).MaximumLength(200);
            RuleFor(x => x.Dto.LastCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.VendorPriority).GreaterThan((short)0);
            RuleFor(x => x.Dto.LeadTimeDays).GreaterThan((short)0).When(x => x.Dto.LeadTimeDays.HasValue);
        });
    }
}

public sealed class UpdateFgsVendorInventoryItemCommandValidator : AbstractValidator<UpdateFgsVendorInventoryItemCommand>
{
    public UpdateFgsVendorInventoryItemCommandValidator(
        IFgsVendorReadRepository vendorReadRepository,
        IFgsVendorInventoryItemReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.VendorId).GreaterThan(0);
            RuleFor(x => x.Dto.VendorId)
                .MustAsync(async (vendorId, cancellationToken) =>
                    await vendorReadRepository.ExistsAsync(vendorId, cancellationToken: cancellationToken))
                .WithMessage("Vendor does not exist.");
            RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId, cancellationToken))
                .WithMessage("Inventory item does not exist.");
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                    !await readRepository.ExistsByVendorAndItemAsync(
                        command.Dto.VendorId,
                        command.Dto.InventoryItemId,
                        command.Id,
                        cancellationToken))
                .WithMessage("A vendor inventory item for this vendor and item already exists.");
            RuleFor(x => x.Dto.VendorPartNumber).MaximumLength(100);
            RuleFor(x => x.Dto.VendorPartName).MaximumLength(200);
            RuleFor(x => x.Dto.LastCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.VendorPriority).GreaterThan((short)0);
            RuleFor(x => x.Dto.LeadTimeDays).GreaterThan((short)0).When(x => x.Dto.LeadTimeDays.HasValue);
        });
    }
}

public sealed class PatchFgsVendorInventoryItemCommandValidator : AbstractValidator<PatchFgsVendorInventoryItemCommand>
{
    public PatchFgsVendorInventoryItemCommandValidator(
        IFgsVendorReadRepository vendorReadRepository,
        IFgsVendorInventoryItemReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.VendorId).GreaterThan(0).When(x => x.Dto.VendorId.HasValue);
            RuleFor(x => x.Dto.VendorId)
                .MustAsync(async (vendorId, cancellationToken) =>
                    await vendorReadRepository.ExistsAsync(vendorId!.Value, cancellationToken: cancellationToken))
                .WithMessage("Vendor does not exist.")
                .When(x => x.Dto.VendorId.HasValue);
            RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0).When(x => x.Dto.InventoryItemId.HasValue);
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId!.Value, cancellationToken))
                .WithMessage("Inventory item does not exist.")
                .When(x => x.Dto.InventoryItemId.HasValue);
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                {
                    var current = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                    if (current is null)
                    {
                        return true;
                    }

                    var vendorId = command.Dto.VendorId ?? current.VendorId;
                    var itemId = command.Dto.InventoryItemId ?? current.InventoryItemId;
                    return !await readRepository.ExistsByVendorAndItemAsync(vendorId, itemId, command.Id, cancellationToken);
                })
                .WithMessage("A vendor inventory item for this vendor and item already exists.")
                .When(x => x.Dto.VendorId.HasValue || x.Dto.InventoryItemId.HasValue);
            RuleFor(x => x.Dto.VendorPartNumber).MaximumLength(100).When(x => x.Dto.VendorPartNumber is not null);
            RuleFor(x => x.Dto.VendorPartName).MaximumLength(200).When(x => x.Dto.VendorPartName is not null);
            RuleFor(x => x.Dto.LastCost).GreaterThanOrEqualTo(0).When(x => x.Dto.LastCost.HasValue);
            RuleFor(x => x.Dto.VendorPriority).GreaterThan((short)0).When(x => x.Dto.VendorPriority.HasValue);
            RuleFor(x => x.Dto.LeadTimeDays).GreaterThan((short)0).When(x => x.Dto.LeadTimeDays.HasValue);
        });
    }
}
