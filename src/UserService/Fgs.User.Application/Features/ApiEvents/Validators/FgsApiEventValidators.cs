using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Features.ApiEvents.Commands.CreateFgsApiEvent;
using Fgs.User.Application.Features.ApiEvents.Commands.PatchFgsApiEvent;
using Fgs.User.Application.Features.ApiEvents.Commands.UpdateFgsApiEvent;
using FluentValidation;

namespace Fgs.User.Application.Features.ApiEvents.Validators;

public sealed class CreateFgsApiEventCommandValidator : AbstractValidator<CreateFgsApiEventCommand>
{
    public CreateFgsApiEventCommandValidator(IFgsApiEventReadRepository readRepository)
    {
        RuleFor(x => x.Dto.EventCode)
            .NotEmpty()
            .MaximumLength(100)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("EventCode must be uppercase.")
            .MustAsync(async (command, eventCode, cancellationToken) =>
                !await readRepository.ExistsByEventCodeAsync(eventCode, null, cancellationToken))
            .WithMessage("An API event with this event code already exists.");

        RuleFor(x => x.Dto.EventCategory)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.EventVersion)
            .GreaterThan((short)0);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0);
    }
}

public sealed class UpdateFgsApiEventCommandValidator : AbstractValidator<UpdateFgsApiEventCommand>
{
    public UpdateFgsApiEventCommandValidator(IFgsApiEventReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.EventCode)
            .NotEmpty()
            .MaximumLength(100)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("EventCode must be uppercase.")
            .MustAsync(async (command, eventCode, cancellationToken) =>
                !await readRepository.ExistsByEventCodeAsync(eventCode, command.Id, cancellationToken))
            .WithMessage("An API event with this event code already exists.");

        RuleFor(x => x.Dto.EventCategory)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.EventVersion)
            .GreaterThan((short)0);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0);
    }
}

public sealed class PatchFgsApiEventCommandValidator : AbstractValidator<PatchFgsApiEventCommand>
{
    public PatchFgsApiEventCommandValidator(IFgsApiEventReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.EventCode)
            .NotEmpty()
            .MaximumLength(100)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("EventCode must be uppercase.")
            .MustAsync(async (command, eventCode, cancellationToken) =>
                !await readRepository.ExistsByEventCodeAsync(eventCode!, command.Id, cancellationToken))
            .WithMessage("An API event with this event code already exists.")
            .When(x => x.Dto.EventCode is not null);

        RuleFor(x => x.Dto.EventCategory)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => x.Dto.EventCategory is not null);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.EventVersion)
            .GreaterThan((short)0)
            .When(x => x.Dto.EventVersion.HasValue);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0)
            .When(x => x.Dto.DisplayOrder.HasValue);
    }
}
