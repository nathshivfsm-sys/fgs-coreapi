using Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.CreateFgsUniversalMatrixOneTimeFee;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.PatchFgsUniversalMatrixOneTimeFee;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.UpdateFgsUniversalMatrixOneTimeFee;
using FluentValidation;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Validators;

public sealed class CreateFgsUniversalMatrixOneTimeFeeCommandValidator : AbstractValidator<CreateFgsUniversalMatrixOneTimeFeeCommand>
{
    public CreateFgsUniversalMatrixOneTimeFeeCommandValidator(IFgsUniversalMatrixOneTimeFeeReadRepository readRepository)
    {
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceIdAndNameAsync(dto.UniversalPricingServiceId, dto.Name, null, cancellationToken))
            .WithMessage("A universal matrix one-time fee with this combination already exists.");
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.Amount).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class UpdateFgsUniversalMatrixOneTimeFeeCommandValidator : AbstractValidator<UpdateFgsUniversalMatrixOneTimeFeeCommand>
{
    public UpdateFgsUniversalMatrixOneTimeFeeCommandValidator(IFgsUniversalMatrixOneTimeFeeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceIdAndNameAsync(dto.UniversalPricingServiceId, dto.Name, command.Id, cancellationToken))
            .WithMessage("A universal matrix one-time fee with this combination already exists.");
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.Amount).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class PatchFgsUniversalMatrixOneTimeFeeCommandValidator : AbstractValidator<PatchFgsUniversalMatrixOneTimeFeeCommand>
{
    public PatchFgsUniversalMatrixOneTimeFeeCommandValidator(IFgsUniversalMatrixOneTimeFeeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceIdAndNameAsync(dto.UniversalPricingServiceId!.Value, dto.Name!, command.Id, cancellationToken))
            .WithMessage("A universal matrix one-time fee with this combination already exists.")
            .When(x => x.Dto.UniversalPricingServiceId.HasValue && x.Dto.Name is not null);
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (_, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value!.Value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.")
            .When(x => x.Dto.UniversalPricingServiceId.HasValue);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name).MaximumLength(150).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Amount).GreaterThanOrEqualTo(0m).When(x => x.Dto.Amount.HasValue);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
