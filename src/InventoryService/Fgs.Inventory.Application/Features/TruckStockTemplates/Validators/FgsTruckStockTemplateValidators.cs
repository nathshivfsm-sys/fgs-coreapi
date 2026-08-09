using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.CreateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.PatchFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.UpdateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Validators;

public sealed class CreateFgsTruckStockTemplateCommandValidator : AbstractValidator<CreateFgsTruckStockTemplateCommand>
{
    public CreateFgsTruckStockTemplateCommandValidator(IFgsTruckStockTemplateReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.TemplateCode).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.TemplateCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("TemplateCode must be uppercase.");
            RuleFor(x => x.Dto.TemplateCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByTemplateCodeAsync(code, null, cancellationToken))
                .WithMessage("A truck stock template with this code already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);

            RuleFor(x => x.Dto.Items)
                .Must(HaveUniqueInventoryItemIds)
                .WithMessage("Items must not contain duplicate inventoryItemId values.")
                .When(x => x.Dto.Items is { Count: > 0 });

            RuleForEach(x => x.Dto.Items)
                .SetValidator(new FgsTruckStockTemplateItemDtoValidator(readRepository))
                .When(x => x.Dto.Items is not null);
        });
    }

    private static bool HaveUniqueInventoryItemIds(IReadOnlyList<FgsTruckStockTemplateItemDto>? items) =>
        items is null
        || items.Select(i => i.InventoryItemId).Distinct().Count() == items.Count;
}

public sealed class UpdateFgsTruckStockTemplateCommandValidator : AbstractValidator<UpdateFgsTruckStockTemplateCommand>
{
    public UpdateFgsTruckStockTemplateCommandValidator(IFgsTruckStockTemplateReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.TemplateCode).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.TemplateCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("TemplateCode must be uppercase.");
            RuleFor(x => x.Dto.TemplateCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByTemplateCodeAsync(code, command.Id, cancellationToken))
                .WithMessage("A truck stock template with this code already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);

            RuleFor(x => x.Dto.Items)
                .Must(HaveUniqueInventoryItemIds)
                .WithMessage("Items must not contain duplicate inventoryItemId values.")
                .When(x => x.Dto.Items is { Count: > 0 });

            RuleForEach(x => x.Dto.Items)
                .SetValidator(new FgsTruckStockTemplateItemDtoValidator(readRepository))
                .When(x => x.Dto.Items is not null);
        });
    }

    private static bool HaveUniqueInventoryItemIds(IReadOnlyList<FgsTruckStockTemplateItemDto>? items) =>
        items is null
        || items.Select(i => i.InventoryItemId).Distinct().Count() == items.Count;
}

public sealed class PatchFgsTruckStockTemplateCommandValidator : AbstractValidator<PatchFgsTruckStockTemplateCommand>
{
    public PatchFgsTruckStockTemplateCommandValidator(IFgsTruckStockTemplateReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.TemplateCode).NotEmpty().MaximumLength(100)
                .When(x => x.Dto.TemplateCode is not null);
            RuleFor(x => x.Dto.TemplateCode!)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("TemplateCode must be uppercase.")
                .When(x => x.Dto.TemplateCode is not null);
            RuleFor(x => x.Dto.TemplateCode!)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByTemplateCodeAsync(code, command.Id, cancellationToken))
                .WithMessage("A truck stock template with this code already exists.")
                .When(x => x.Dto.TemplateCode is not null);
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200)
                .When(x => x.Dto.Name is not null);

            RuleFor(x => x.Dto.Items)
                .Must(HaveUniqueInventoryItemIds)
                .WithMessage("Items must not contain duplicate inventoryItemId values.")
                .When(x => x.Dto.Items is { Count: > 0 });

            RuleForEach(x => x.Dto.Items)
                .SetValidator(new FgsTruckStockTemplateItemDtoValidator(readRepository))
                .When(x => x.Dto.Items is not null);
        });
    }

    private static bool HaveUniqueInventoryItemIds(IReadOnlyList<FgsTruckStockTemplateItemDto>? items) =>
        items is null
        || items.Select(i => i.InventoryItemId).Distinct().Count() == items.Count;
}

internal sealed class FgsTruckStockTemplateItemDtoValidator : AbstractValidator<FgsTruckStockTemplateItemDto>
{
    public FgsTruckStockTemplateItemDtoValidator(IFgsTruckStockTemplateReadRepository readRepository)
    {
        RuleFor(x => x.Id!.Value).GreaterThan(0)
            .When(x => x.Id.HasValue);
        RuleFor(x => x.InventoryItemId).GreaterThan(0);
        RuleFor(x => x.InventoryItemId)
            .MustAsync(async (inventoryItemId, cancellationToken) =>
                await readRepository.ExistsInventoryItemAsync(inventoryItemId, cancellationToken))
            .WithMessage("Inventory item was not found or is inactive.");
        RuleFor(x => x.TargetQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MinimumQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x)
            .Must(dto => dto.TargetQuantity >= dto.MinimumQuantity)
            .WithMessage("TargetQuantity must be greater than or equal to MinimumQuantity.");
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0);
    }
}
