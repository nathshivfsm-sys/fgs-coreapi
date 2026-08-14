using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.CreateFgsInventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.DeleteFgsInventoryItemAlternate;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.UpdateFgsInventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Validators;

public sealed class CreateFgsInventoryItemAlternatesCommandValidator
    : AbstractValidator<CreateFgsInventoryItemAlternatesCommand>
{
    public CreateFgsInventoryItemAlternatesCommandValidator(IFgsInventoryItemReadRepository readRepository)
    {
        RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
        RuleFor(x => x.Dto.InventoryItemId)
            .MustAsync(async (id, cancellationToken) =>
                await readRepository.ExistsAsync(id, activeOnly: false, cancellationToken))
            .WithMessage("Inventory item was not found.");
        RuleFor(x => x.Dto.Items).NotNull();
        RuleFor(x => x.Dto.Items)
            .Must(HaveUniqueAlternateInventoryItemIds)
            .WithMessage("Items must not contain duplicate alternateInventoryItemId values.")
            .When(x => x.Dto.Items is { Count: > 0 });
        RuleForEach(x => x.Dto.Items)
            .SetValidator(new FgsInventoryItemAlternateDtoValidator(readRepository));
    }

    private static bool HaveUniqueAlternateInventoryItemIds(IReadOnlyList<FgsInventoryItemAlternateDto>? items) =>
        items is null
        || items.Select(a => a.AlternateInventoryItemId).Distinct().Count() == items.Count;
}

public sealed class UpdateFgsInventoryItemAlternatesCommandValidator
    : AbstractValidator<UpdateFgsInventoryItemAlternatesCommand>
{
    public UpdateFgsInventoryItemAlternatesCommandValidator(IFgsInventoryItemReadRepository readRepository)
    {
        RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
        RuleFor(x => x.Dto.InventoryItemId)
            .MustAsync(async (id, cancellationToken) =>
                await readRepository.ExistsAsync(id, activeOnly: false, cancellationToken))
            .WithMessage("Inventory item was not found.");
        RuleFor(x => x.Dto.Items).NotNull();
        RuleFor(x => x.Dto.Items)
            .Must(HaveUniqueAlternateInventoryItemIds)
            .WithMessage("Items must not contain duplicate alternateInventoryItemId values.")
            .When(x => x.Dto.Items is { Count: > 0 });
        RuleForEach(x => x.Dto.Items)
            .SetValidator(new FgsInventoryItemAlternateDtoValidator(readRepository));
    }

    private static bool HaveUniqueAlternateInventoryItemIds(IReadOnlyList<FgsInventoryItemAlternateDto>? items) =>
        items is null
        || items.Select(a => a.AlternateInventoryItemId).Distinct().Count() == items.Count;
}

public sealed class DeleteFgsInventoryItemAlternateCommandValidator
    : AbstractValidator<DeleteFgsInventoryItemAlternateCommand>
{
    public DeleteFgsInventoryItemAlternateCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
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
