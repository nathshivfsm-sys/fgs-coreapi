using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.CreateFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.PatchFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.UpdateFgsUniversalPricingService;
using FluentValidation;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Validators;

public sealed class CreateFgsUniversalPricingServiceCommandValidator : AbstractValidator<CreateFgsUniversalPricingServiceCommand>
{
    public CreateFgsUniversalPricingServiceCommandValidator(IFgsUniversalPricingServiceReadRepository readRepository)
    {
        RuleFor(x => x.Dto.UniversalPricingServiceCode).NotEmpty();
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MaximumLength(50);
        RuleFor(x => x.Dto.UniversalPricingServiceCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("UniversalPricingServiceCode must be uppercase.");
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MustAsync(async (_, code, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceCodeAsync(code, null, cancellationToken))
            .WithMessage("A universal pricing service with this code already exists.");
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class UpdateFgsUniversalPricingServiceCommandValidator : AbstractValidator<UpdateFgsUniversalPricingServiceCommand>
{
    public UpdateFgsUniversalPricingServiceCommandValidator(IFgsUniversalPricingServiceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceCode).NotEmpty();
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MaximumLength(50);
        RuleFor(x => x.Dto.UniversalPricingServiceCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("UniversalPricingServiceCode must be uppercase.");
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A universal pricing service with this code already exists.");
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class PatchFgsUniversalPricingServiceCommandValidator : AbstractValidator<PatchFgsUniversalPricingServiceCommand>
{
    public PatchFgsUniversalPricingServiceCommandValidator(IFgsUniversalPricingServiceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceCode).NotEmpty().When(x => x.Dto.UniversalPricingServiceCode is not null);
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MaximumLength(50).When(x => x.Dto.UniversalPricingServiceCode is not null);
        RuleFor(x => x.Dto.UniversalPricingServiceCode)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("UniversalPricingServiceCode must be uppercase.")
            .When(x => x.Dto.UniversalPricingServiceCode is not null);
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A universal pricing service with this code already exists.")
            .When(x => x.Dto.UniversalPricingServiceCode is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
