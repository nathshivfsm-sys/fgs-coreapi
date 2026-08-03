using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.CreateFgsUniversalMatrixFrequencyDiscount;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.PatchFgsUniversalMatrixFrequencyDiscount;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.UpdateFgsUniversalMatrixFrequencyDiscount;
using FluentValidation;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Validators;

public sealed class CreateFgsUniversalMatrixFrequencyDiscountCommandValidator : AbstractValidator<CreateFgsUniversalMatrixFrequencyDiscountCommand>
{
    public CreateFgsUniversalMatrixFrequencyDiscountCommandValidator(IFgsUniversalMatrixFrequencyDiscountReadRepository readRepository)
    {
        RuleFor(x => x.Dto.UniversalPricingServiceId).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (_, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(100);
        RuleFor(x => x.Dto.DiscountPercent).InclusiveBetween(0m, 100m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, value, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(command.Dto.UniversalPricingServiceId, value, null, cancellationToken))
            .WithMessage("A universal matrix frequency discount with this name already exists for the universal pricing service.");
    }
}

public sealed class UpdateFgsUniversalMatrixFrequencyDiscountCommandValidator : AbstractValidator<UpdateFgsUniversalMatrixFrequencyDiscountCommand>
{
    public UpdateFgsUniversalMatrixFrequencyDiscountCommandValidator(IFgsUniversalMatrixFrequencyDiscountReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceId).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (_, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(100);
        RuleFor(x => x.Dto.DiscountPercent).InclusiveBetween(0m, 100m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, value, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(command.Dto.UniversalPricingServiceId, value, command.Id, cancellationToken))
            .WithMessage("A universal matrix frequency discount with this name already exists for the universal pricing service.");
    }
}

public sealed class PatchFgsUniversalMatrixFrequencyDiscountCommandValidator : AbstractValidator<PatchFgsUniversalMatrixFrequencyDiscountCommand>
{
    public PatchFgsUniversalMatrixFrequencyDiscountCommandValidator(IFgsUniversalMatrixFrequencyDiscountReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceId).GreaterThan(0).When(x => x.Dto.UniversalPricingServiceId.HasValue);
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (_, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsUniversalPricingServiceIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.")
            .When(x => x.Dto.UniversalPricingServiceId.HasValue);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name).MaximumLength(100).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.DiscountPercent).InclusiveBetween(0m, 100m).When(x => x.Dto.DiscountPercent.HasValue);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
