using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.CreateFgsInventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.DeleteFgsInventoryItemDependency;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.UpdateFgsInventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Validators;

public sealed class CreateFgsInventoryItemDependenciesCommandValidator
    : AbstractValidator<CreateFgsInventoryItemDependenciesCommand>
{
    public CreateFgsInventoryItemDependenciesCommandValidator(IFgsInventoryItemReadRepository readRepository)
    {
        RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
        RuleFor(x => x.Dto.InventoryItemId)
            .MustAsync(async (id, cancellationToken) =>
                await readRepository.ExistsAsync(id, activeOnly: false, cancellationToken))
            .WithMessage("Inventory item was not found.");
        RuleFor(x => x.Dto.Items).NotNull();
        RuleFor(x => x.Dto.Items)
            .Must(HaveUniqueDependentInventoryItemIds)
            .WithMessage("Items must not contain duplicate dependentInventoryItemId values.")
            .When(x => x.Dto.Items is { Count: > 0 });
        RuleForEach(x => x.Dto.Items)
            .SetValidator(new FgsInventoryItemDependencyDtoValidator(readRepository));
    }

    private static bool HaveUniqueDependentInventoryItemIds(IReadOnlyList<FgsInventoryItemDependencyDto>? items) =>
        items is null
        || items.Select(d => d.DependentInventoryItemId).Distinct().Count() == items.Count;
}

public sealed class UpdateFgsInventoryItemDependenciesCommandValidator
    : AbstractValidator<UpdateFgsInventoryItemDependenciesCommand>
{
    public UpdateFgsInventoryItemDependenciesCommandValidator(IFgsInventoryItemReadRepository readRepository)
    {
        RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
        RuleFor(x => x.Dto.InventoryItemId)
            .MustAsync(async (id, cancellationToken) =>
                await readRepository.ExistsAsync(id, activeOnly: false, cancellationToken))
            .WithMessage("Inventory item was not found.");
        RuleFor(x => x.Dto.Items).NotNull();
        RuleFor(x => x.Dto.Items)
            .Must(HaveUniqueDependentInventoryItemIds)
            .WithMessage("Items must not contain duplicate dependentInventoryItemId values.")
            .When(x => x.Dto.Items is { Count: > 0 });
        RuleForEach(x => x.Dto.Items)
            .SetValidator(new FgsInventoryItemDependencyDtoValidator(readRepository));
    }

    private static bool HaveUniqueDependentInventoryItemIds(IReadOnlyList<FgsInventoryItemDependencyDto>? items) =>
        items is null
        || items.Select(d => d.DependentInventoryItemId).Distinct().Count() == items.Count;
}

public sealed class DeleteFgsInventoryItemDependencyCommandValidator
    : AbstractValidator<DeleteFgsInventoryItemDependencyCommand>
{
    public DeleteFgsInventoryItemDependencyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
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
