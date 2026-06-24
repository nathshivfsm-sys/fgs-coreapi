using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.CreateLeadDisqualificationReason;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.PatchLeadDisqualificationReason;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.UpdateLeadDisqualificationReason;
using FluentValidation;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Validators;

public sealed class CreateLeadDisqualificationReasonCommandValidator : AbstractValidator<CreateLeadDisqualificationReasonCommand>
{
    public CreateLeadDisqualificationReasonCommandValidator(ILeadDisqualificationReasonReadRepository readRepository)
    {
        RuleFor(x => x.Dto.ReasonCode).NotEmpty();
        RuleFor(x => x.Dto.ReasonCode).MaximumLength(50);
        RuleFor(x => x.Dto.ReasonCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ReasonCode must be uppercase.");
        RuleFor(x => x.Dto.ReasonCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByReasonCodeAsync(code, null, cancellationToken))
            .WithMessage("A lead disqualification reason with this code already exists.");
        RuleFor(x => x.Dto.ReasonName).NotEmpty();
        RuleFor(x => x.Dto.ReasonName).MaximumLength(100);
        RuleFor(x => x.Dto.ReasonName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByReasonNameAsync(name, null, cancellationToken))
            .WithMessage("An active lead disqualification reason with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
        RuleFor(x => x.Dto.ReasonCode).NotEmpty();
        RuleFor(x => x.Dto.ReasonCode).MaximumLength(50);
        RuleFor(x => x.Dto.ReasonCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ReasonCode must be uppercase.");
        RuleFor(x => x.Dto.ReasonCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByReasonCodeAsync(code, null, cancellationToken))
            .WithMessage("A lead disqualification reason with this code already exists.");
        RuleFor(x => x.Dto.ReasonName).NotEmpty();
        RuleFor(x => x.Dto.ReasonName).MaximumLength(100);
        RuleFor(x => x.Dto.ReasonName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByReasonNameAsync(name, null, cancellationToken))
            .WithMessage("An active lead disqualification reason with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);

    }
}

public sealed class UpdateLeadDisqualificationReasonCommandValidator : AbstractValidator<UpdateLeadDisqualificationReasonCommand>
{
    public UpdateLeadDisqualificationReasonCommandValidator(ILeadDisqualificationReasonReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.ReasonCode).NotEmpty();
        RuleFor(x => x.Dto.ReasonCode).MaximumLength(50);
        RuleFor(x => x.Dto.ReasonCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ReasonCode must be uppercase.");
        RuleFor(x => x.Dto.ReasonCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByReasonCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A lead disqualification reason with this code already exists.");
        RuleFor(x => x.Dto.ReasonName).NotEmpty();
        RuleFor(x => x.Dto.ReasonName).MaximumLength(100);
        RuleFor(x => x.Dto.ReasonName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByReasonNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active lead disqualification reason with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);

    }
}

public sealed class PatchLeadDisqualificationReasonCommandValidator : AbstractValidator<PatchLeadDisqualificationReasonCommand>
{
    public PatchLeadDisqualificationReasonCommandValidator(ILeadDisqualificationReasonReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.ReasonCode).NotEmpty();
        RuleFor(x => x.Dto.ReasonCode).MaximumLength(50);
        RuleFor(x => x.Dto.ReasonCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ReasonCode must be uppercase.");
        RuleFor(x => x.Dto.ReasonCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByReasonCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A lead disqualification reason with this code already exists.");
        RuleFor(x => x.Dto.ReasonName).NotEmpty();
        RuleFor(x => x.Dto.ReasonName).MaximumLength(100);
        RuleFor(x => x.Dto.ReasonName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByReasonNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active lead disqualification reason with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);

    }
}
