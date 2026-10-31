using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.CreateFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.PatchFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.UpdateFgsSetupPriceBookVersion;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Validators;

public sealed class CreateFgsSetupPriceBookVersionCommandValidator
    : AbstractValidator<CreateFgsSetupPriceBookVersionCommand>
{
    public CreateFgsSetupPriceBookVersionCommandValidator(IFgsSetupPriceBookVersionReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Version)
            .NotEmpty()
            .MaximumLength(25)
            .MustAsync(async (command, version, cancellationToken) =>
                !await readRepository.ExistsByVersionAsync(version, null, cancellationToken))
            .WithMessage("A price book version with this version already exists.");

        RuleFor(x => x.Dto)
            .Must(dto => dto.EffectiveTo is null || dto.EffectiveTo >= dto.EffectiveFrom)
            .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom.");
    }
}

public sealed class UpdateFgsSetupPriceBookVersionCommandValidator
    : AbstractValidator<UpdateFgsSetupPriceBookVersionCommand>
{
    public UpdateFgsSetupPriceBookVersionCommandValidator(IFgsSetupPriceBookVersionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Version)
            .NotEmpty()
            .MaximumLength(25)
            .MustAsync(async (command, version, cancellationToken) =>
                !await readRepository.ExistsByVersionAsync(version, command.Id, cancellationToken))
            .WithMessage("A price book version with this version already exists.");

        RuleFor(x => x.Dto)
            .Must(dto => dto.EffectiveTo is null || dto.EffectiveTo >= dto.EffectiveFrom)
            .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom.");
    }
}

public sealed class PatchFgsSetupPriceBookVersionCommandValidator
    : AbstractValidator<PatchFgsSetupPriceBookVersionCommand>
{
    public PatchFgsSetupPriceBookVersionCommandValidator(IFgsSetupPriceBookVersionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Version)
            .NotEmpty()
            .MaximumLength(25)
            .MustAsync(async (command, version, cancellationToken) =>
                !await readRepository.ExistsByVersionAsync(version!, command.Id, cancellationToken))
            .WithMessage("A price book version with this version already exists.")
            .When(x => x.Dto.Version is not null);

        RuleFor(x => x.Dto)
            .Must(dto =>
                dto.EffectiveTo is null
                || dto.EffectiveFrom is null
                || dto.EffectiveTo >= dto.EffectiveFrom)
            .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom.")
            .When(x => x.Dto.EffectiveFrom.HasValue || x.Dto.EffectiveTo.HasValue);
    }
}
