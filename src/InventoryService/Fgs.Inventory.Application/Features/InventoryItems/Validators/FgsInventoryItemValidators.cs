using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.CreateFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.PatchFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.UpdateFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
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

            RuleFor(x => x.Dto.Alternates)
                .Must(HaveUniqueAlternateInventoryItemIds)
                .WithMessage("Alternates must not contain duplicate alternateInventoryItemId values.")
                .When(x => x.Dto.Alternates is { Count: > 0 });
            RuleFor(x => x.Dto.Dependencies)
                .Must(HaveUniqueDependentInventoryItemIds)
                .WithMessage("Dependencies must not contain duplicate dependentInventoryItemId values.")
                .When(x => x.Dto.Dependencies is { Count: > 0 });

            RuleForEach(x => x.Dto.Alternates)
                .SetValidator(new FgsInventoryItemAlternateDtoValidator(readRepository))
                .When(x => x.Dto.Alternates is not null);
            RuleForEach(x => x.Dto.Dependencies)
                .SetValidator(new FgsInventoryItemDependencyDtoValidator(readRepository))
                .When(x => x.Dto.Dependencies is not null);
        });
    }

    private static bool HaveUniqueAlternateInventoryItemIds(IReadOnlyList<FgsInventoryItemAlternateDto>? alternates) =>
        alternates is null
        || alternates.Select(a => a.AlternateInventoryItemId).Distinct().Count() == alternates.Count;

    private static bool HaveUniqueDependentInventoryItemIds(IReadOnlyList<FgsInventoryItemDependencyDto>? dependencies) =>
        dependencies is null
        || dependencies.Select(d => d.DependentInventoryItemId).Distinct().Count() == dependencies.Count;
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

            RuleFor(x => x.Dto.Alternates)
                .Must(HaveUniqueAlternateInventoryItemIds)
                .WithMessage("Alternates must not contain duplicate alternateInventoryItemId values.")
                .When(x => x.Dto.Alternates is { Count: > 0 });
            RuleFor(x => x.Dto.Dependencies)
                .Must(HaveUniqueDependentInventoryItemIds)
                .WithMessage("Dependencies must not contain duplicate dependentInventoryItemId values.")
                .When(x => x.Dto.Dependencies is { Count: > 0 });

            RuleForEach(x => x.Dto.Alternates)
                .SetValidator(new FgsInventoryItemAlternateDtoValidator(readRepository))
                .When(x => x.Dto.Alternates is not null);
            RuleForEach(x => x.Dto.Dependencies)
                .SetValidator(new FgsInventoryItemDependencyDtoValidator(readRepository))
                .When(x => x.Dto.Dependencies is not null);
        });
    }

    private static bool HaveUniqueAlternateInventoryItemIds(IReadOnlyList<FgsInventoryItemAlternateDto>? alternates) =>
        alternates is null
        || alternates.Select(a => a.AlternateInventoryItemId).Distinct().Count() == alternates.Count;

    private static bool HaveUniqueDependentInventoryItemIds(IReadOnlyList<FgsInventoryItemDependencyDto>? dependencies) =>
        dependencies is null
        || dependencies.Select(d => d.DependentInventoryItemId).Distinct().Count() == dependencies.Count;
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

            RuleFor(x => x.Dto.Alternates)
                .Must(HaveUniqueAlternateInventoryItemIds)
                .WithMessage("Alternates must not contain duplicate alternateInventoryItemId values.")
                .When(x => x.Dto.Alternates is { Count: > 0 });
            RuleFor(x => x.Dto.Dependencies)
                .Must(HaveUniqueDependentInventoryItemIds)
                .WithMessage("Dependencies must not contain duplicate dependentInventoryItemId values.")
                .When(x => x.Dto.Dependencies is { Count: > 0 });

            RuleForEach(x => x.Dto.Alternates)
                .SetValidator(new FgsInventoryItemAlternateDtoValidator(readRepository))
                .When(x => x.Dto.Alternates is not null);
            RuleForEach(x => x.Dto.Dependencies)
                .SetValidator(new FgsInventoryItemDependencyDtoValidator(readRepository))
                .When(x => x.Dto.Dependencies is not null);
        });
    }

    private static bool HaveUniqueAlternateInventoryItemIds(IReadOnlyList<FgsInventoryItemAlternateDto>? alternates) =>
        alternates is null
        || alternates.Select(a => a.AlternateInventoryItemId).Distinct().Count() == alternates.Count;

    private static bool HaveUniqueDependentInventoryItemIds(IReadOnlyList<FgsInventoryItemDependencyDto>? dependencies) =>
        dependencies is null
        || dependencies.Select(d => d.DependentInventoryItemId).Distinct().Count() == dependencies.Count;
}

internal sealed class FgsInventoryItemAlternateDtoValidator : AbstractValidator<FgsInventoryItemAlternateDto>
{
    public FgsInventoryItemAlternateDtoValidator(IFgsInventoryItemReadRepository readRepository)
    {
        RuleFor(x => x.Id!.Value).GreaterThan(0)
            .When(x => x.Id.HasValue);
        RuleFor(x => x.AlternateInventoryItemId).GreaterThan(0);
        RuleFor(x => x.AlternateInventoryItemId)
            .MustAsync(async (alternateInventoryItemId, cancellationToken) =>
                await readRepository.ExistsInventoryItemAsync(alternateInventoryItemId, cancellationToken))
            .WithMessage("Alternate inventory item was not found or is inactive.");
        RuleFor(x => x.PriorityOrder).GreaterThan((short)0);
    }
}

internal sealed class FgsInventoryItemDependencyDtoValidator : AbstractValidator<FgsInventoryItemDependencyDto>
{
    public FgsInventoryItemDependencyDtoValidator(IFgsInventoryItemReadRepository readRepository)
    {
        RuleFor(x => x.Id!.Value).GreaterThan(0)
            .When(x => x.Id.HasValue);
        RuleFor(x => x.DependentInventoryItemId).GreaterThan(0);
        RuleFor(x => x.DependentInventoryItemId)
            .MustAsync(async (dependentInventoryItemId, cancellationToken) =>
                await readRepository.ExistsInventoryItemAsync(dependentInventoryItemId, cancellationToken))
            .WithMessage("Dependent inventory item was not found or is inactive.");
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.DisplayOrder).GreaterThan((short)0);
    }
}
