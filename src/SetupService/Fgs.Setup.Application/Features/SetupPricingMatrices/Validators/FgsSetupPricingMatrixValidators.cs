using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.CreateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.PatchFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.UpdateFgsSetupPricingMatrix;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Validators;

public sealed class CreateFgsSetupPricingMatrixCommandValidator : AbstractValidator<CreateFgsSetupPricingMatrixCommand>
{
    public CreateFgsSetupPricingMatrixCommandValidator(IFgsSetupPricingMatrixReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.Name)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("Name (code) must be uppercase.");
            RuleFor(x => x.Dto.Description).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.Name).MustAsync(async (_, code, cancellationToken) =>
                    !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
                .WithMessage("A pricing matrix with this code already exists.");
            RuleFor(x => x.Dto.PriceAdjustmentTypeId)
                .InclusiveBetween((short)1, (short)3)
                .When(x => x.Dto.PriceAdjustmentTypeId.HasValue);
            RuleFor(x => x.Dto)
                .Must(dto => dto.EffectiveTo is null || dto.EffectiveFrom is null || dto.EffectiveTo >= dto.EffectiveFrom)
                .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom.");
        });
    }
}

public sealed class UpdateFgsSetupPricingMatrixCommandValidator : AbstractValidator<UpdateFgsSetupPricingMatrixCommand>
{
    public UpdateFgsSetupPricingMatrixCommandValidator(IFgsSetupPricingMatrixReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.Name)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("Name (code) must be uppercase.");
            RuleFor(x => x.Dto.Description).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.Name).MustAsync(async (command, code, cancellationToken) =>
                    !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
                .WithMessage("A pricing matrix with this code already exists.");
            RuleFor(x => x.Dto.PriceAdjustmentTypeId)
                .InclusiveBetween((short)1, (short)3)
                .When(x => x.Dto.PriceAdjustmentTypeId.HasValue);
            RuleFor(x => x.Dto)
                .Must(dto => dto.EffectiveTo is null || dto.EffectiveFrom is null || dto.EffectiveTo >= dto.EffectiveFrom)
                .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom.");
        });
    }
}

public sealed class PatchFgsSetupPricingMatrixCommandValidator : AbstractValidator<PatchFgsSetupPricingMatrixCommand>
{
    public PatchFgsSetupPricingMatrixCommandValidator(IFgsSetupPricingMatrixReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .MaximumLength(50)
                .When(x => x.Dto.Name is not null);
            RuleFor(x => x.Dto.Name)
                .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .When(x => x.Dto.Name is not null)
                .WithMessage("Name (code) must be uppercase.");
            RuleFor(x => x.Dto.Description)
                .NotEmpty()
                .MaximumLength(200)
                .When(x => x.Dto.Description is not null);
            RuleFor(x => x.Dto.PriceAdjustmentTypeId)
                .InclusiveBetween((short)1, (short)3)
                .When(x => x.Dto.PriceAdjustmentTypeId.HasValue);
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                    command.Dto.Name is null ||
                    !await readRepository.ExistsByCodeAsync(command.Dto.Name, command.Id, cancellationToken))
                .WithMessage("A pricing matrix with this code already exists.");
        });
    }
}
