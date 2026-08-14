using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.CreateFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.PatchFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.UpdateFgsInventoryItem;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventoryItems.Validators;

public sealed class CreateFgsInventoryItemCommandValidator : AbstractValidator<CreateFgsInventoryItemCommand>
{
    public CreateFgsInventoryItemCommandValidator(
        IFgsInventoryItemReadRepository readRepository,
        IFgsInventoryItemTypeReadRepository itemTypeReadRepository,
        IFgsInventoryCategoryReadRepository categoryReadRepository,
        IFgsInventorySubCategoryReadRepository subCategoryReadRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables like {{inventoryItemTypeId}} produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.ItemCode).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.ItemCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("ItemCode must be uppercase.");
            RuleFor(x => x.Dto.ItemCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByItemCodeAsync(code, null, cancellationToken))
                .WithMessage("An inventory item with this code already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.InventoryItemTypeId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryItemTypeId)
                .MustAsync(async (typeId, cancellationToken) =>
                    await itemTypeReadRepository.ExistsAsync(typeId, cancellationToken: cancellationToken))
                .WithMessage("Inventory item type was not found or is inactive.");
            RuleFor(x => x.Dto.InventoryCategoryId!.Value)
                .GreaterThan(0)
                .MustAsync(async (categoryId, cancellationToken) =>
                    await categoryReadRepository.ExistsAsync(categoryId, cancellationToken: cancellationToken))
                .WithMessage("Inventory category was not found or is inactive.")
                .When(x => x.Dto.InventoryCategoryId.HasValue);
            RuleFor(x => x.Dto.InventorySubCategoryId!.Value)
                .GreaterThan(0)
                .MustAsync(async (subCategoryId, cancellationToken) =>
                    await subCategoryReadRepository.ExistsAsync(subCategoryId, cancellationToken: cancellationToken))
                .WithMessage("Inventory sub-category was not found or is inactive.")
                .When(x => x.Dto.InventorySubCategoryId.HasValue);
        });
    }
}

public sealed class UpdateFgsInventoryItemCommandValidator : AbstractValidator<UpdateFgsInventoryItemCommand>
{
    public UpdateFgsInventoryItemCommandValidator(
        IFgsInventoryItemReadRepository readRepository,
        IFgsInventoryItemTypeReadRepository itemTypeReadRepository,
        IFgsInventoryCategoryReadRepository categoryReadRepository,
        IFgsInventorySubCategoryReadRepository subCategoryReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.ItemCode).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.ItemCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("ItemCode must be uppercase.");
            RuleFor(x => x.Dto.ItemCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByItemCodeAsync(code, command.Id, cancellationToken))
                .WithMessage("An inventory item with this code already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.InventoryItemTypeId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryItemTypeId)
                .MustAsync(async (typeId, cancellationToken) =>
                    await itemTypeReadRepository.ExistsAsync(typeId, cancellationToken: cancellationToken))
                .WithMessage("Inventory item type was not found or is inactive.");
            RuleFor(x => x.Dto.InventoryCategoryId!.Value)
                .GreaterThan(0)
                .MustAsync(async (categoryId, cancellationToken) =>
                    await categoryReadRepository.ExistsAsync(categoryId, cancellationToken: cancellationToken))
                .WithMessage("Inventory category was not found or is inactive.")
                .When(x => x.Dto.InventoryCategoryId.HasValue);
            RuleFor(x => x.Dto.InventorySubCategoryId!.Value)
                .GreaterThan(0)
                .MustAsync(async (subCategoryId, cancellationToken) =>
                    await subCategoryReadRepository.ExistsAsync(subCategoryId, cancellationToken: cancellationToken))
                .WithMessage("Inventory sub-category was not found or is inactive.")
                .When(x => x.Dto.InventorySubCategoryId.HasValue);
        });
    }
}

public sealed class PatchFgsInventoryItemCommandValidator : AbstractValidator<PatchFgsInventoryItemCommand>
{
    public PatchFgsInventoryItemCommandValidator(
        IFgsInventoryItemReadRepository readRepository,
        IFgsInventoryItemTypeReadRepository itemTypeReadRepository,
        IFgsInventoryCategoryReadRepository categoryReadRepository,
        IFgsInventorySubCategoryReadRepository subCategoryReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.ItemCode).NotEmpty().MaximumLength(100)
                .When(x => x.Dto.ItemCode is not null);
            RuleFor(x => x.Dto.ItemCode!)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("ItemCode must be uppercase.")
                .When(x => x.Dto.ItemCode is not null);
            RuleFor(x => x.Dto.ItemCode!)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByItemCodeAsync(code, command.Id, cancellationToken))
                .WithMessage("An inventory item with this code already exists.")
                .When(x => x.Dto.ItemCode is not null);
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200)
                .When(x => x.Dto.Name is not null);
            RuleFor(x => x.Dto.InventoryItemTypeId!.Value)
                .GreaterThan(0)
                .MustAsync(async (typeId, cancellationToken) =>
                    await itemTypeReadRepository.ExistsAsync(typeId, cancellationToken: cancellationToken))
                .WithMessage("Inventory item type was not found or is inactive.")
                .When(x => x.Dto.InventoryItemTypeId.HasValue);
            RuleFor(x => x.Dto.InventoryCategoryId!.Value)
                .GreaterThan(0)
                .MustAsync(async (categoryId, cancellationToken) =>
                    await categoryReadRepository.ExistsAsync(categoryId, cancellationToken: cancellationToken))
                .WithMessage("Inventory category was not found or is inactive.")
                .When(x => x.Dto.InventoryCategoryId.HasValue);
            RuleFor(x => x.Dto.InventorySubCategoryId!.Value)
                .GreaterThan(0)
                .MustAsync(async (subCategoryId, cancellationToken) =>
                    await subCategoryReadRepository.ExistsAsync(subCategoryId, cancellationToken: cancellationToken))
                .WithMessage("Inventory sub-category was not found or is inactive.")
                .When(x => x.Dto.InventorySubCategoryId.HasValue);
        });
    }
}
