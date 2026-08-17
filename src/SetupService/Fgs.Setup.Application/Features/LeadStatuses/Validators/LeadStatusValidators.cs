using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.CreateLeadStatus;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.PatchLeadStatus;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.UpdateLeadStatus;
using FluentValidation;

namespace Fgs.Setup.Application.Features.LeadStatuses.Validators;

public sealed class CreateLeadStatusCommandValidator : AbstractValidator<CreateLeadStatusCommand>
{
    public CreateLeadStatusCommandValidator(ILeadStatusReadRepository readRepository)
    {
        RuleFor(x => x.Dto.StatusCode).NotEmpty();
        RuleFor(x => x.Dto.StatusCode).MaximumLength(50);
        RuleFor(x => x.Dto.StatusCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("StatusCode must be uppercase.");
        RuleFor(x => x.Dto.StatusCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByStatusCodeAsync(code, null, cancellationToken))
            .WithMessage("A lead status with this code already exists.");
        RuleFor(x => x.Dto.StatusName).NotEmpty();
        RuleFor(x => x.Dto.StatusName).MaximumLength(100);
        RuleFor(x => x.Dto.StatusName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByStatusNameAsync(name, null, cancellationToken))
            .WithMessage("An active lead status with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
        RuleFor(x => x.Dto.StatusCode).NotEmpty();
        RuleFor(x => x.Dto.StatusCode).MaximumLength(50);
        RuleFor(x => x.Dto.StatusCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("StatusCode must be uppercase.");
        RuleFor(x => x.Dto.StatusCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByStatusCodeAsync(code, null, cancellationToken))
            .WithMessage("A lead status with this code already exists.");
        RuleFor(x => x.Dto.StatusName).NotEmpty();
        RuleFor(x => x.Dto.StatusName).MaximumLength(100);
        RuleFor(x => x.Dto.StatusName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByStatusNameAsync(name, null, cancellationToken))
            .WithMessage("An active lead status with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);

    }
}

public sealed class UpdateLeadStatusCommandValidator : AbstractValidator<UpdateLeadStatusCommand>
{
    public UpdateLeadStatusCommandValidator(ILeadStatusReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.StatusCode).NotEmpty();
        RuleFor(x => x.Dto.StatusCode).MaximumLength(50);
        RuleFor(x => x.Dto.StatusCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("StatusCode must be uppercase.");
        RuleFor(x => x.Dto.StatusCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByStatusCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A lead status with this code already exists.");
        RuleFor(x => x.Dto.StatusName).NotEmpty();
        RuleFor(x => x.Dto.StatusName).MaximumLength(100);
        RuleFor(x => x.Dto.StatusName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByStatusNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active lead status with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);

    }
}

public sealed class PatchLeadStatusCommandValidator : AbstractValidator<PatchLeadStatusCommand>
{
    public PatchLeadStatusCommandValidator(ILeadStatusReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.StatusCode).NotEmpty().When(x => x.Dto.StatusCode is not null);
        RuleFor(x => x.Dto.StatusCode).MaximumLength(50).When(x => x.Dto.StatusCode is not null);
        RuleFor(x => x.Dto.StatusCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("StatusCode must be uppercase.").When(x => x.Dto.StatusCode is not null);
        RuleFor(x => x.Dto.StatusCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByStatusCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A lead status with this code already exists.").When(x => x.Dto.StatusCode is not null);
        RuleFor(x => x.Dto.StatusName).NotEmpty().When(x => x.Dto.StatusName is not null);
        RuleFor(x => x.Dto.StatusName).MaximumLength(100).When(x => x.Dto.StatusName is not null);
        RuleFor(x => x.Dto.StatusName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByStatusNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active lead status with this name already exists.").When(x => x.Dto.StatusName is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);

    }
}
