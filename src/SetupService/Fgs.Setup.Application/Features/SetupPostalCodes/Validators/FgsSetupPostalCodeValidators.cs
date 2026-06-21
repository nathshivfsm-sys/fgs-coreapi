using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Commands.CreateFgsSetupPostalCode;
using Fgs.Setup.Application.Features.SetupPostalCodes.Commands.PatchFgsSetupPostalCode;
using Fgs.Setup.Application.Features.SetupPostalCodes.Commands.UpdateFgsSetupPostalCode;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Validators;

public sealed class CreateFgsSetupPostalCodeCommandValidator : AbstractValidator<CreateFgsSetupPostalCodeCommand>
{
    public CreateFgsSetupPostalCodeCommandValidator(IFgsSetupPostalCodeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.PostalCode).NotEmpty();
        RuleFor(x => x.Dto.PostalCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByPostalCodeAsync(code, null, cancellationToken))
            .WithMessage("A postal code with this code already exists.");
        RuleFor(x => x.Dto.FgsSetupZoneId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsZoneIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified zone was not found.");
        RuleFor(x => x.Dto.FgsSetupTaxId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTaxIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tax was not found.");        RuleFor(x => x.Dto.PostalCode).NotEmpty();
        RuleFor(x => x.Dto.PostalCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByPostalCodeAsync(code, null, cancellationToken))
            .WithMessage("A postal code with this code already exists.");
        RuleFor(x => x.Dto.FgsSetupZoneId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsZoneIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified zone was not found.");
        RuleFor(x => x.Dto.FgsSetupTaxId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTaxIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tax was not found.");
    }
}

public sealed class UpdateFgsSetupPostalCodeCommandValidator : AbstractValidator<UpdateFgsSetupPostalCodeCommand>
{
    public UpdateFgsSetupPostalCodeCommandValidator(IFgsSetupPostalCodeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.PostalCode).NotEmpty();
        RuleFor(x => x.Dto.PostalCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByPostalCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A postal code with this code already exists.");
        RuleFor(x => x.Dto.FgsSetupZoneId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsZoneIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified zone was not found.");
        RuleFor(x => x.Dto.FgsSetupTaxId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTaxIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tax was not found.");
    }
}

public sealed class PatchFgsSetupPostalCodeCommandValidator : AbstractValidator<PatchFgsSetupPostalCodeCommand>
{
    public PatchFgsSetupPostalCodeCommandValidator(IFgsSetupPostalCodeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.PostalCode).NotEmpty();
        RuleFor(x => x.Dto.PostalCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByPostalCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A postal code with this code already exists.");
        RuleFor(x => x.Dto.FgsSetupZoneId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsZoneIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified zone was not found.").When(x => x.Dto.FgsSetupZoneId.HasValue);
        RuleFor(x => x.Dto.FgsSetupTaxId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTaxIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tax was not found.").When(x => x.Dto.FgsSetupTaxId.HasValue);
    }
}
