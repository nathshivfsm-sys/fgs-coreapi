using Fgs.Inventory.Application.Abstractions.InventoryTransactions;
using Fgs.Inventory.Application.Features.InventoryTransactions.Commands.CreateFgsInventoryTransaction;
using Fgs.Inventory.Domain.Entities;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventoryTransactions.Validators;

public sealed class CreateFgsInventoryTransactionCommandValidator : AbstractValidator<CreateFgsInventoryTransactionCommand>
{
    private static readonly HashSet<string> AllowedTransactionTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        InventoryTransactionTypes.Initial,
        InventoryTransactionTypes.PurchaseReceipt,
        InventoryTransactionTypes.Transfer,
        InventoryTransactionTypes.Usage,
        InventoryTransactionTypes.Adjustment,
        InventoryTransactionTypes.Return,
        InventoryTransactionTypes.PhysicalCount
    };

    public CreateFgsInventoryTransactionCommandValidator(IFgsInventoryTransactionReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.TransactionNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.TransactionNumber)
                .MustAsync(async (number, cancellationToken) =>
                    !await readRepository.ExistsByTransactionNumberAsync(number, null, cancellationToken))
                .WithMessage("A transaction with this number already exists.");
            RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId, cancellationToken))
                .WithMessage("Inventory item does not exist.");
            RuleFor(x => x.Dto.TransactionType).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Dto.TransactionType)
                .Must(type => AllowedTransactionTypes.Contains(type))
                .WithMessage("TransactionType is not valid.");
            RuleFor(x => x.Dto.Quantity).NotEqual(0);
            RuleFor(x => x.Dto.UnitCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.ReferenceType).MaximumLength(30);
        });
    }
}
