using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.CreateFgsUniversalMatrixSizeTier;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.PatchFgsUniversalMatrixSizeTier;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.UpdateFgsUniversalMatrixSizeTier;
using FluentValidation;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Validators;

public sealed class CreateFgsUniversalMatrixSizeTierCommandValidator : AbstractValidator<CreateFgsUniversalMatrixSizeTierCommand>
{
    public CreateFgsUniversalMatrixSizeTierCommandValidator(IFgsUniversalMatrixSizeTierReadRepository readRepository)
    {
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceIdAndNameAsync(dto.UniversalPricingServiceId, dto.Name, null, cancellationToken))
            .WithMessage("A universal matrix size tier with this combination already exists.");
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(100);
        RuleFor(x => x.Dto.Multiplier).GreaterThan(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class UpdateFgsUniversalMatrixSizeTierCommandValidator : AbstractValidator<UpdateFgsUniversalMatrixSizeTierCommand>
{
    public UpdateFgsUniversalMatrixSizeTierCommandValidator(IFgsUniversalMatrixSizeTierReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceIdAndNameAsync(dto.UniversalPricingServiceId, dto.Name, command.Id, cancellationToken))
            .WithMessage("A universal matrix size tier with this combination already exists.");
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(100);
        RuleFor(x => x.Dto.Multiplier).GreaterThan(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class PatchFgsUniversalMatrixSizeTierCommandValidator : AbstractValidator<PatchFgsUniversalMatrixSizeTierCommand>
{
    public PatchFgsUniversalMatrixSizeTierCommandValidator(IFgsUniversalMatrixSizeTierReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceIdAndNameAsync(dto.UniversalPricingServiceId!.Value, dto.Name!, command.Id, cancellationToken))
            .WithMessage("A universal matrix size tier with this combination already exists.")
            .When(x => x.Dto.UniversalPricingServiceId.HasValue && x.Dto.Name is not null);
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (_, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value!.Value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.")
            .When(x => x.Dto.UniversalPricingServiceId.HasValue);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name).MaximumLength(100).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Multiplier).GreaterThan(0m).When(x => x.Dto.Multiplier.HasValue);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
