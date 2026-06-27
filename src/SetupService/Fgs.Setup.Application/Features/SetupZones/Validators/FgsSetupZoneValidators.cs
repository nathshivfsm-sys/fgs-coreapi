using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Commands.CreateFgsSetupZone;
using Fgs.Setup.Application.Features.SetupZones.Commands.PatchFgsSetupZone;
using Fgs.Setup.Application.Features.SetupZones.Commands.UpdateFgsSetupZone;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupZones.Validators;

public sealed class CreateFgsSetupZoneCommandValidator : AbstractValidator<CreateFgsSetupZoneCommand>
{
    public CreateFgsSetupZoneCommandValidator(IFgsSetupZoneReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A zone with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A zone with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();

    }
}

public sealed class UpdateFgsSetupZoneCommandValidator : AbstractValidator<UpdateFgsSetupZoneCommand>
{
    public UpdateFgsSetupZoneCommandValidator(IFgsSetupZoneReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A zone with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();

    }
}

public sealed class PatchFgsSetupZoneCommandValidator : AbstractValidator<PatchFgsSetupZoneCommand>
{
    public PatchFgsSetupZoneCommandValidator(IFgsSetupZoneReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A zone with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();

    }
}
