using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Features.InventoryStocks.Commands.CreateFgsInventoryStock;
using Fgs.Inventory.Application.Features.InventoryStocks.Commands.PatchFgsInventoryStock;
using Fgs.Inventory.Application.Features.InventoryStocks.Commands.UpdateFgsInventoryStock;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Validators;

public sealed class CreateFgsInventoryStockCommandValidator : AbstractValidator<CreateFgsInventoryStockCommand>
{
    public CreateFgsInventoryStockCommandValidator(IFgsInventoryStockReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId, cancellationToken))
                .WithMessage("Inventory item does not exist.");
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    !await readRepository.ExistsByInventoryItemIdAsync(itemId, null, cancellationToken))
                .WithMessage("Inventory stock for this item already exists.");
            RuleFor(x => x.Dto.QuantityOnHand).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.QuantityCommitted).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.QuantityAvailable).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.AverageCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.LastCost).GreaterThanOrEqualTo(0);
        });
    }
}

public sealed class UpdateFgsInventoryStockCommandValidator : AbstractValidator<UpdateFgsInventoryStockCommand>
{
    public UpdateFgsInventoryStockCommandValidator(IFgsInventoryStockReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId, cancellationToken))
                .WithMessage("Inventory item does not exist.");
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                    !await readRepository.ExistsByInventoryItemIdAsync(
                        command.Dto.InventoryItemId,
                        command.Id,
                        cancellationToken))
                .WithMessage("Inventory stock for this item already exists.");
            RuleFor(x => x.Dto.QuantityOnHand).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.QuantityCommitted).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.QuantityAvailable).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.AverageCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.LastCost).GreaterThanOrEqualTo(0);
        });
    }
}

public sealed class PatchFgsInventoryStockCommandValidator : AbstractValidator<PatchFgsInventoryStockCommand>
{
    public PatchFgsInventoryStockCommandValidator(IFgsInventoryStockReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0).When(x => x.Dto.InventoryItemId.HasValue);
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId!.Value, cancellationToken))
                .WithMessage("Inventory item does not exist.")
                .When(x => x.Dto.InventoryItemId.HasValue);
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                {
                    if (!command.Dto.InventoryItemId.HasValue)
                    {
                        return true;
                    }

                    return !await readRepository.ExistsByInventoryItemIdAsync(
                        command.Dto.InventoryItemId.Value,
                        command.Id,
                        cancellationToken);
                })
                .WithMessage("Inventory stock for this item already exists.")
                .When(x => x.Dto.InventoryItemId.HasValue);
            RuleFor(x => x.Dto.QuantityOnHand).GreaterThanOrEqualTo(0).When(x => x.Dto.QuantityOnHand.HasValue);
            RuleFor(x => x.Dto.QuantityCommitted).GreaterThanOrEqualTo(0).When(x => x.Dto.QuantityCommitted.HasValue);
            RuleFor(x => x.Dto.QuantityAvailable).GreaterThanOrEqualTo(0).When(x => x.Dto.QuantityAvailable.HasValue);
            RuleFor(x => x.Dto.AverageCost).GreaterThanOrEqualTo(0).When(x => x.Dto.AverageCost.HasValue);
            RuleFor(x => x.Dto.LastCost).GreaterThanOrEqualTo(0).When(x => x.Dto.LastCost.HasValue);
        });
    }
}
