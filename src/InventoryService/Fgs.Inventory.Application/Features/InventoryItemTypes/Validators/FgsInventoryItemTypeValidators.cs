using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.CreateFgsInventoryItemType;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.PatchFgsInventoryItemType;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.UpdateFgsInventoryItemType;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Validators;

public sealed class CreateFgsInventoryItemTypeCommandValidator : AbstractValidator<CreateFgsInventoryItemTypeCommand>
{
    public CreateFgsInventoryItemTypeCommandValidator(IFgsInventoryItemTypeReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.ItemTypeCode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.ItemTypeCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("ItemTypeCode must be uppercase.");
            RuleFor(x => x.Dto.ItemTypeCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByItemTypeCodeAsync(code, null, cancellationToken))
                .WithMessage("An inventory item type with this code already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
        });
    }
}

public sealed class UpdateFgsInventoryItemTypeCommandValidator : AbstractValidator<UpdateFgsInventoryItemTypeCommand>
{
    public UpdateFgsInventoryItemTypeCommandValidator(IFgsInventoryItemTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.ItemTypeCode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.ItemTypeCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("ItemTypeCode must be uppercase.");
            RuleFor(x => x.Dto.ItemTypeCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByItemTypeCodeAsync(code, command.Id, cancellationToken))
                .WithMessage("An inventory item type with this code already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
        });
    }
}

public sealed class PatchFgsInventoryItemTypeCommandValidator : AbstractValidator<PatchFgsInventoryItemTypeCommand>
{
    public PatchFgsInventoryItemTypeCommandValidator(IFgsInventoryItemTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.ItemTypeCode).NotEmpty().MaximumLength(50)
                .When(x => x.Dto.ItemTypeCode is not null);
            RuleFor(x => x.Dto.ItemTypeCode!)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("ItemTypeCode must be uppercase.")
                .When(x => x.Dto.ItemTypeCode is not null);
            RuleFor(x => x.Dto.ItemTypeCode!)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByItemTypeCodeAsync(code, command.Id, cancellationToken))
                .WithMessage("An inventory item type with this code already exists.")
                .When(x => x.Dto.ItemTypeCode is not null);
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200)
                .When(x => x.Dto.Name is not null);
            RuleFor(x => x.Dto.DisplayOrder!.Value).GreaterThanOrEqualTo((short)0)
                .When(x => x.Dto.DisplayOrder.HasValue);
        });
    }
}
