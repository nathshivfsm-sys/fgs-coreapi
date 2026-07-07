using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.CreateFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.PatchFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.UpdateFgsUniversalMatrixAddOn;
using FluentValidation;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Validators;

public sealed class CreateFgsUniversalMatrixAddOnCommandValidator : AbstractValidator<CreateFgsUniversalMatrixAddOnCommand>
{
    public CreateFgsUniversalMatrixAddOnCommandValidator(IFgsUniversalMatrixAddOnReadRepository readRepository)
    {
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceIdAndNameAsync(dto.UniversalPricingServiceId, dto.Name, null, cancellationToken))
            .WithMessage("A universal matrix add-on with this combination already exists.");
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.UnitType).NotEmpty();
        RuleFor(x => x.Dto.UnitType).MaximumLength(50);
        RuleFor(x => x.Dto.Price).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class UpdateFgsUniversalMatrixAddOnCommandValidator : AbstractValidator<UpdateFgsUniversalMatrixAddOnCommand>
{
    public UpdateFgsUniversalMatrixAddOnCommandValidator(IFgsUniversalMatrixAddOnReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceIdAndNameAsync(dto.UniversalPricingServiceId, dto.Name, command.Id, cancellationToken))
            .WithMessage("A universal matrix add-on with this combination already exists.");
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.UnitType).NotEmpty();
        RuleFor(x => x.Dto.UnitType).MaximumLength(50);
        RuleFor(x => x.Dto.Price).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class PatchFgsUniversalMatrixAddOnCommandValidator : AbstractValidator<PatchFgsUniversalMatrixAddOnCommand>
{
    public PatchFgsUniversalMatrixAddOnCommandValidator(IFgsUniversalMatrixAddOnReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceIdAndNameAsync(dto.UniversalPricingServiceId!.Value, dto.Name!, command.Id, cancellationToken))
            .WithMessage("A universal matrix add-on with this combination already exists.")
            .When(x => x.Dto.UniversalPricingServiceId.HasValue && x.Dto.Name is not null);
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (_, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value!.Value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.")
            .When(x => x.Dto.UniversalPricingServiceId.HasValue);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name).MaximumLength(150).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.UnitType).NotEmpty().When(x => x.Dto.UnitType is not null);
        RuleFor(x => x.Dto.UnitType).MaximumLength(50).When(x => x.Dto.UnitType is not null);
        RuleFor(x => x.Dto.Price).GreaterThanOrEqualTo(0m).When(x => x.Dto.Price.HasValue);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
