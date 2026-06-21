using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Commands.CreateFgsSetupTaxDetail;
using Fgs.Setup.Application.Features.SetupTaxDetails.Commands.PatchFgsSetupTaxDetail;
using Fgs.Setup.Application.Features.SetupTaxDetails.Commands.UpdateFgsSetupTaxDetail;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Validators;

public sealed class CreateFgsSetupTaxDetailCommandValidator : AbstractValidator<CreateFgsSetupTaxDetailCommand>
{
    public CreateFgsSetupTaxDetailCommandValidator(IFgsSetupTaxDetailReadRepository readRepository)
    {
        RuleFor(x => x.Dto.FgsSetupTaxId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsTaxIdAsync(value, cancellationToken))
            .WithMessage("The specified tax was not found.");
        RuleFor(x => x.Dto.FgsSetupTaxAuthorityId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsTaxAuthorityIdAsync(value, cancellationToken))
            .WithMessage("The specified tax authority was not found.");




        RuleFor(x => x.Dto.TaxPercent).InclusiveBetween(0m, 100m);
        RuleFor(x => x.Dto).Must(dto => !dto.EffectiveToDate.HasValue || dto.EffectiveToDate.Value >= dto.EffectiveFromDate)
            .WithMessage("EffectiveToDate must be greater than or equal to EffectiveFromDate.");
    }
}

public sealed class UpdateFgsSetupTaxDetailCommandValidator : AbstractValidator<UpdateFgsSetupTaxDetailCommand>
{
    public UpdateFgsSetupTaxDetailCommandValidator(IFgsSetupTaxDetailReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsSetupTaxId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsTaxIdAsync(value, cancellationToken))
            .WithMessage("The specified tax was not found.");
        RuleFor(x => x.Dto.FgsSetupTaxAuthorityId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsTaxAuthorityIdAsync(value, cancellationToken))
            .WithMessage("The specified tax authority was not found.");




        RuleFor(x => x.Dto.TaxPercent).InclusiveBetween(0m, 100m);
        RuleFor(x => x.Dto).Must(dto => !dto.EffectiveToDate.HasValue || dto.EffectiveToDate.Value >= dto.EffectiveFromDate)
            .WithMessage("EffectiveToDate must be greater than or equal to EffectiveFromDate.");
    }
}

public sealed class PatchFgsSetupTaxDetailCommandValidator : AbstractValidator<PatchFgsSetupTaxDetailCommand>
{
    public PatchFgsSetupTaxDetailCommandValidator(IFgsSetupTaxDetailReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsSetupTaxId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTaxIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tax was not found.").When(x => x.Dto.FgsSetupTaxId.HasValue);
        RuleFor(x => x.Dto.FgsSetupTaxAuthorityId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTaxAuthorityIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tax authority was not found.").When(x => x.Dto.FgsSetupTaxAuthorityId.HasValue);




        RuleFor(x => x.Dto.TaxPercent).InclusiveBetween(0m, 100m);
        RuleFor(x => x.Dto).Must(dto => !dto.EffectiveToDate.HasValue || dto.EffectiveToDate.Value >= dto.EffectiveFromDate)
            .WithMessage("EffectiveToDate must be greater than or equal to EffectiveFromDate.");
    }
}
