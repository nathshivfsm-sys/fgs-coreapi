using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Commands.CreateFgsInventoryCategory;
using Fgs.Inventory.Application.Features.InventoryCategories.Commands.PatchFgsInventoryCategory;
using Fgs.Inventory.Application.Features.InventoryCategories.Commands.UpdateFgsInventoryCategory;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Validators;

public sealed class CreateFgsInventoryCategoryCommandValidator : AbstractValidator<CreateFgsInventoryCategoryCommand>
{
    public CreateFgsInventoryCategoryCommandValidator(IFgsInventoryCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.CategoryCode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.CategoryCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("CategoryCode must be uppercase.");
            RuleFor(x => x.Dto.CategoryCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByCategoryCodeAsync(code, null, cancellationToken))
                .WithMessage("An inventory category with this code already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Dto.TextColor).MaximumLength(20);
            RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
            RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
        });
    }
}

public sealed class UpdateFgsInventoryCategoryCommandValidator : AbstractValidator<UpdateFgsInventoryCategoryCommand>
{
    public UpdateFgsInventoryCategoryCommandValidator(IFgsInventoryCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.CategoryCode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.CategoryCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("CategoryCode must be uppercase.");
            RuleFor(x => x.Dto.CategoryCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByCategoryCodeAsync(code, command.Id, cancellationToken))
                .WithMessage("An inventory category with this code already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Dto.TextColor).MaximumLength(20);
            RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
            RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
        });
    }
}

public sealed class PatchFgsInventoryCategoryCommandValidator : AbstractValidator<PatchFgsInventoryCategoryCommand>
{
    public PatchFgsInventoryCategoryCommandValidator(IFgsInventoryCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.CategoryCode).NotEmpty().MaximumLength(50)
                .When(x => x.Dto.CategoryCode is not null);
            RuleFor(x => x.Dto.CategoryCode!)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("CategoryCode must be uppercase.")
                .When(x => x.Dto.CategoryCode is not null);
            RuleFor(x => x.Dto.CategoryCode!)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByCategoryCodeAsync(code, command.Id, cancellationToken))
                .WithMessage("An inventory category with this code already exists.")
                .When(x => x.Dto.CategoryCode is not null);
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(150)
                .When(x => x.Dto.Name is not null);
            RuleFor(x => x.Dto.TextColor).MaximumLength(20)
                .When(x => x.Dto.TextColor is not null);
            RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20)
                .When(x => x.Dto.BackgroundColor is not null);
            RuleFor(x => x.Dto.DisplayOrder!.Value).GreaterThanOrEqualTo((short)0)
                .When(x => x.Dto.DisplayOrder.HasValue);
        });
    }
}
