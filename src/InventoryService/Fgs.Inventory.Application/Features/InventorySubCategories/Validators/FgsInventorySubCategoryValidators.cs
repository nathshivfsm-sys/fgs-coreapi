using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Commands.CreateFgsInventorySubCategory;
using Fgs.Inventory.Application.Features.InventorySubCategories.Commands.PatchFgsInventorySubCategory;
using Fgs.Inventory.Application.Features.InventorySubCategories.Commands.UpdateFgsInventorySubCategory;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Validators;

public sealed class CreateFgsInventorySubCategoryCommandValidator : AbstractValidator<CreateFgsInventorySubCategoryCommand>
{
    public CreateFgsInventorySubCategoryCommandValidator(
        IFgsInventorySubCategoryReadRepository readRepository,
        IFgsInventoryCategoryReadRepository categoryReadRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InventoryCategoryId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryCategoryId)
                .MustAsync(async (categoryId, cancellationToken) =>
                    await categoryReadRepository.ExistsAsync(categoryId, activeOnly: true, cancellationToken))
                .WithMessage("Inventory category was not found or is inactive.");
            RuleFor(x => x.Dto.SubCategoryCode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.SubCategoryCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("SubCategoryCode must be uppercase.");
            RuleFor(x => x.Dto.SubCategoryCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsBySubCategoryCodeAsync(
                        command.Dto.InventoryCategoryId, code, null, cancellationToken))
                .WithMessage("An inventory sub-category with this code already exists for the category.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Dto.TextColor).MaximumLength(20);
            RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
            RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
        });
    }
}

public sealed class UpdateFgsInventorySubCategoryCommandValidator : AbstractValidator<UpdateFgsInventorySubCategoryCommand>
{
    public UpdateFgsInventorySubCategoryCommandValidator(
        IFgsInventorySubCategoryReadRepository readRepository,
        IFgsInventoryCategoryReadRepository categoryReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InventoryCategoryId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryCategoryId)
                .MustAsync(async (categoryId, cancellationToken) =>
                    await categoryReadRepository.ExistsAsync(categoryId, activeOnly: true, cancellationToken))
                .WithMessage("Inventory category was not found or is inactive.");
            RuleFor(x => x.Dto.SubCategoryCode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.SubCategoryCode)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("SubCategoryCode must be uppercase.");
            RuleFor(x => x.Dto.SubCategoryCode)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsBySubCategoryCodeAsync(
                        command.Dto.InventoryCategoryId, code, command.Id, cancellationToken))
                .WithMessage("An inventory sub-category with this code already exists for the category.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Dto.TextColor).MaximumLength(20);
            RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
            RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
        });
    }
}

public sealed class PatchFgsInventorySubCategoryCommandValidator : AbstractValidator<PatchFgsInventorySubCategoryCommand>
{
    public PatchFgsInventorySubCategoryCommandValidator(
        IFgsInventorySubCategoryReadRepository readRepository,
        IFgsInventoryCategoryReadRepository categoryReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InventoryCategoryId!.Value).GreaterThan(0)
                .When(x => x.Dto.InventoryCategoryId.HasValue);
            RuleFor(x => x.Dto.InventoryCategoryId!.Value)
                .MustAsync(async (categoryId, cancellationToken) =>
                    await categoryReadRepository.ExistsAsync(categoryId, activeOnly: true, cancellationToken))
                .WithMessage("Inventory category was not found or is inactive.")
                .When(x => x.Dto.InventoryCategoryId.HasValue);
            RuleFor(x => x.Dto.SubCategoryCode).NotEmpty().MaximumLength(50)
                .When(x => x.Dto.SubCategoryCode is not null);
            RuleFor(x => x.Dto.SubCategoryCode!)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("SubCategoryCode must be uppercase.")
                .When(x => x.Dto.SubCategoryCode is not null);
            RuleFor(x => x.Dto.SubCategoryCode!)
                .MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsBySubCategoryCodeAsync(
                        command.Dto.InventoryCategoryId!.Value, code, command.Id, cancellationToken))
                .WithMessage("An inventory sub-category with this code already exists for the category.")
                .When(x => x.Dto.SubCategoryCode is not null && x.Dto.InventoryCategoryId.HasValue);
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
