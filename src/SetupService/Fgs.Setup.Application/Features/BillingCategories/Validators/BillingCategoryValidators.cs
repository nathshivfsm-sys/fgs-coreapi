using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.BillingCategories.Commands.CreateBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Commands.PatchBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Commands.UpdateBillingCategory;
using FluentValidation;

namespace Fgs.Setup.Application.Features.BillingCategories.Validators;

public sealed class CreateBillingCategoryCommandValidator : AbstractValidator<CreateBillingCategoryCommand>
{
    public CreateBillingCategoryCommandValidator(
        IBillingCategoryReadRepository readRepository,
        IGloBillingCategoryReadRepository gloBillingCategoryReadRepository)
    {
        RuleFor(x => x.Dto.BillingCategoryType).NotEmpty();
        RuleFor(x => x.Dto.BillingCategoryType).MaximumLength(2);
        RuleFor(x => x.Dto.BillingCategoryType)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("BillingCategoryType must be uppercase.");
        RuleFor(x => x.Dto.BillingCategoryType)
            .MustAsync(async (type, cancellationToken) =>
                await gloBillingCategoryReadRepository.ExistsByBillingCategoryTypeAsync(type, cancellationToken))
            .WithMessage("BillingCategoryType is not a valid billing category type.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.BillingCategoryType));
        RuleFor(x => x.Dto)
            .MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByBillingCategoryTypeAndBillingCategoryNameAsync(
                    dto.BillingCategoryType,
                    dto.BillingCategoryName,
                    null,
                    cancellationToken))
            .WithMessage("A billing category with this combination already exists.");
        RuleFor(x => x.Dto.BillingCategoryName).NotEmpty();
        RuleFor(x => x.Dto.BillingCategoryName).MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(700);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class UpdateBillingCategoryCommandValidator : AbstractValidator<UpdateBillingCategoryCommand>
{
    public UpdateBillingCategoryCommandValidator(
        IBillingCategoryReadRepository readRepository,
        IGloBillingCategoryReadRepository gloBillingCategoryReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.BillingCategoryType).NotEmpty();
        RuleFor(x => x.Dto.BillingCategoryType).MaximumLength(2);
        RuleFor(x => x.Dto.BillingCategoryType)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("BillingCategoryType must be uppercase.");
        RuleFor(x => x.Dto.BillingCategoryType)
            .MustAsync(async (type, cancellationToken) =>
                await gloBillingCategoryReadRepository.ExistsByBillingCategoryTypeAsync(type, cancellationToken))
            .WithMessage("BillingCategoryType is not a valid billing category type.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.BillingCategoryType));
        RuleFor(x => x.Dto)
            .MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByBillingCategoryTypeAndBillingCategoryNameAsync(
                    dto.BillingCategoryType,
                    dto.BillingCategoryName,
                    command.Id,
                    cancellationToken))
            .WithMessage("A billing category with this combination already exists.");
        RuleFor(x => x.Dto.BillingCategoryName).NotEmpty();
        RuleFor(x => x.Dto.BillingCategoryName).MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(700);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class PatchBillingCategoryCommandValidator : AbstractValidator<PatchBillingCategoryCommand>
{
    public PatchBillingCategoryCommandValidator(
        IBillingCategoryReadRepository readRepository,
        IGloBillingCategoryReadRepository gloBillingCategoryReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.BillingCategoryType).NotEmpty().When(x => x.Dto.BillingCategoryType is not null);
        RuleFor(x => x.Dto.BillingCategoryType).MaximumLength(2).When(x => x.Dto.BillingCategoryType is not null);
        RuleFor(x => x.Dto.BillingCategoryType)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("BillingCategoryType must be uppercase.")
            .When(x => x.Dto.BillingCategoryType is not null);
        RuleFor(x => x.Dto.BillingCategoryType)
            .MustAsync(async (type, cancellationToken) =>
                type is not null
                && await gloBillingCategoryReadRepository.ExistsByBillingCategoryTypeAsync(type, cancellationToken))
            .WithMessage("BillingCategoryType is not a valid billing category type.")
            .When(x => x.Dto.BillingCategoryType is not null);
        RuleFor(x => x.Dto)
            .MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByBillingCategoryTypeAndBillingCategoryNameAsync(
                    dto.BillingCategoryType!,
                    dto.BillingCategoryName!,
                    command.Id,
                    cancellationToken))
            .WithMessage("A billing category with this combination already exists.")
            .When(x => x.Dto.BillingCategoryType is not null && x.Dto.BillingCategoryName is not null);
        RuleFor(x => x.Dto.BillingCategoryName).NotEmpty().When(x => x.Dto.BillingCategoryName is not null);
        RuleFor(x => x.Dto.BillingCategoryName).MaximumLength(100).When(x => x.Dto.BillingCategoryName is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(700).When(x => x.Dto.Description is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
