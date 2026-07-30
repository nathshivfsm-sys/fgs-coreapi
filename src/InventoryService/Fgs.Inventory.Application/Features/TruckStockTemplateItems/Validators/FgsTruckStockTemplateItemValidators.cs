using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.CreateFgsTruckStockTemplateItem;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.PatchFgsTruckStockTemplateItem;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.UpdateFgsTruckStockTemplateItem;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Validators;

public sealed class CreateFgsTruckStockTemplateItemCommandValidator : AbstractValidator<CreateFgsTruckStockTemplateItemCommand>
{
    public CreateFgsTruckStockTemplateItemCommandValidator(
        IFgsTruckStockTemplateReadRepository templateReadRepository,
        IFgsTruckStockTemplateItemReadRepository itemReadRepository)
    {
        RuleFor(x => x.TemplateId).GreaterThan(0);
        RuleFor(x => x.TemplateId)
            .MustAsync(async (templateId, cancellationToken) =>
                await templateReadRepository.ExistsAsync(templateId, activeOnly: true, cancellationToken))
            .WithMessage("Truck stock template was not found or is inactive.");
        RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
        RuleFor(x => x.Dto.InventoryItemId)
            .MustAsync(async (inventoryItemId, cancellationToken) =>
                await itemReadRepository.ExistsInventoryItemAsync(inventoryItemId, cancellationToken))
            .WithMessage("Inventory item was not found or is inactive.");
        RuleFor(x => x.Dto.InventoryItemId)
            .MustAsync(async (command, inventoryItemId, cancellationToken) =>
                !await itemReadRepository.ExistsByTemplateAndItemAsync(
                    command.TemplateId, inventoryItemId, null, cancellationToken))
            .WithMessage("This inventory item is already on the truck stock template.");
        RuleFor(x => x.Dto.TargetQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Dto.MinimumQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Dto)
            .Must(dto => dto.TargetQuantity >= dto.MinimumQuantity)
            .WithMessage("TargetQuantity must be greater than or equal to MinimumQuantity.");
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo(0);
    }
}

public sealed class UpdateFgsTruckStockTemplateItemCommandValidator : AbstractValidator<UpdateFgsTruckStockTemplateItemCommand>
{
    public UpdateFgsTruckStockTemplateItemCommandValidator(
        IFgsTruckStockTemplateReadRepository templateReadRepository,
        IFgsTruckStockTemplateItemReadRepository itemReadRepository)
    {
        RuleFor(x => x.TemplateId).GreaterThan(0);
        RuleFor(x => x.ItemId).GreaterThan(0);
        RuleFor(x => x.TemplateId)
            .MustAsync(async (templateId, cancellationToken) =>
                await templateReadRepository.ExistsAsync(templateId, activeOnly: false, cancellationToken))
            .WithMessage("Truck stock template was not found.");
        RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
        RuleFor(x => x.Dto.InventoryItemId)
            .MustAsync(async (inventoryItemId, cancellationToken) =>
                await itemReadRepository.ExistsInventoryItemAsync(inventoryItemId, cancellationToken))
            .WithMessage("Inventory item was not found or is inactive.");
        RuleFor(x => x.Dto.InventoryItemId)
            .MustAsync(async (command, inventoryItemId, cancellationToken) =>
                !await itemReadRepository.ExistsByTemplateAndItemAsync(
                    command.TemplateId, inventoryItemId, command.ItemId, cancellationToken))
            .WithMessage("This inventory item is already on the truck stock template.");
        RuleFor(x => x.Dto.TargetQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Dto.MinimumQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Dto)
            .Must(dto => dto.TargetQuantity >= dto.MinimumQuantity)
            .WithMessage("TargetQuantity must be greater than or equal to MinimumQuantity.");
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo(0);
    }
}

public sealed class PatchFgsTruckStockTemplateItemCommandValidator : AbstractValidator<PatchFgsTruckStockTemplateItemCommand>
{
    public PatchFgsTruckStockTemplateItemCommandValidator(
        IFgsTruckStockTemplateReadRepository templateReadRepository,
        IFgsTruckStockTemplateItemReadRepository itemReadRepository)
    {
        RuleFor(x => x.TemplateId).GreaterThan(0);
        RuleFor(x => x.ItemId).GreaterThan(0);
        RuleFor(x => x.TemplateId)
            .MustAsync(async (templateId, cancellationToken) =>
                await templateReadRepository.ExistsAsync(templateId, activeOnly: false, cancellationToken))
            .WithMessage("Truck stock template was not found.");
        RuleFor(x => x.Dto.InventoryItemId!).GreaterThan(0)
            .When(x => x.Dto.InventoryItemId.HasValue);
        RuleFor(x => x.Dto.InventoryItemId!.Value)
            .MustAsync(async (inventoryItemId, cancellationToken) =>
                await itemReadRepository.ExistsInventoryItemAsync(inventoryItemId, cancellationToken))
            .WithMessage("Inventory item was not found or is inactive.")
            .When(x => x.Dto.InventoryItemId.HasValue);
        RuleFor(x => x.Dto.InventoryItemId!.Value)
            .MustAsync(async (command, inventoryItemId, cancellationToken) =>
                !await itemReadRepository.ExistsByTemplateAndItemAsync(
                    command.TemplateId, inventoryItemId, command.ItemId, cancellationToken))
            .WithMessage("This inventory item is already on the truck stock template.")
            .When(x => x.Dto.InventoryItemId.HasValue);
        RuleFor(x => x.Dto.TargetQuantity!.Value).GreaterThanOrEqualTo(0)
            .When(x => x.Dto.TargetQuantity.HasValue);
        RuleFor(x => x.Dto.MinimumQuantity!.Value).GreaterThanOrEqualTo(0)
            .When(x => x.Dto.MinimumQuantity.HasValue);
        RuleFor(x => x)
            .Must(command =>
            {
                if (!command.Dto.TargetQuantity.HasValue && !command.Dto.MinimumQuantity.HasValue)
                {
                    return true;
                }

                // Cross-field validation for patch is best-effort when both values are supplied.
                if (command.Dto.TargetQuantity.HasValue && command.Dto.MinimumQuantity.HasValue)
                {
                    return command.Dto.TargetQuantity.Value >= command.Dto.MinimumQuantity.Value;
                }

                return true;
            })
            .WithMessage("TargetQuantity must be greater than or equal to MinimumQuantity.");
        RuleFor(x => x.Dto.DisplayOrder!.Value).GreaterThanOrEqualTo(0)
            .When(x => x.Dto.DisplayOrder.HasValue);
    }
}
