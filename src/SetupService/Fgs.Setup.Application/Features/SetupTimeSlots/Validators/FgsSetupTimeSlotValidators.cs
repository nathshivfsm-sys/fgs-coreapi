using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Commands.CreateFgsSetupTimeSlot;
using Fgs.Setup.Application.Features.SetupTimeSlots.Commands.PatchFgsSetupTimeSlot;
using Fgs.Setup.Application.Features.SetupTimeSlots.Commands.UpdateFgsSetupTimeSlot;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Validators;

public sealed class CreateFgsSetupTimeSlotCommandValidator : AbstractValidator<CreateFgsSetupTimeSlotCommand>
{
    public CreateFgsSetupTimeSlotCommandValidator(IFgsSetupTimeSlotReadRepository readRepository)
    {
        RuleFor(x => x.Dto.FgsSetupZoneId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsZoneIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified zone was not found.");
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A time slot with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();






        RuleFor(x => x.Dto).Must(dto => dto.EndTime > dto.BeginTime)
            .WithMessage("EndTime must be greater than BeginTime.");
    }
}

public sealed class UpdateFgsSetupTimeSlotCommandValidator : AbstractValidator<UpdateFgsSetupTimeSlotCommand>
{
    public UpdateFgsSetupTimeSlotCommandValidator(IFgsSetupTimeSlotReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsSetupZoneId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsZoneIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified zone was not found.");
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A time slot with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();






        RuleFor(x => x.Dto).Must(dto => dto.EndTime > dto.BeginTime)
            .WithMessage("EndTime must be greater than BeginTime.");
    }
}

public sealed class PatchFgsSetupTimeSlotCommandValidator : AbstractValidator<PatchFgsSetupTimeSlotCommand>
{
    public PatchFgsSetupTimeSlotCommandValidator(IFgsSetupTimeSlotReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsSetupZoneId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsZoneIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified zone was not found.").When(x => x.Dto.FgsSetupZoneId.HasValue);
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A time slot with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();






        RuleFor(x => x.Dto).Must(dto => dto.EndTime > dto.BeginTime)
            .WithMessage("EndTime must be greater than BeginTime.");
    }
}
