using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.CreateFgsSalesDispositionReason;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.PatchFgsSalesDispositionReason;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.UpdateFgsSalesDispositionReason;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Validators;

public sealed class CreateFgsSalesDispositionReasonCommandValidator : AbstractValidator<CreateFgsSalesDispositionReasonCommand>
{
    public CreateFgsSalesDispositionReasonCommandValidator(IFgsSalesDispositionReasonReadRepository readRepository)
    {
        RuleFor(x => x.Dto.DispositionReasonCode).NotEmpty();
        RuleFor(x => x.Dto.DispositionReasonCode).MaximumLength(50);
        RuleFor(x => x.Dto.DispositionReasonCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("DispositionReasonCode must be uppercase.");
        RuleFor(x => x.Dto.DispositionReasonCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByDispositionReasonCodeAsync(code, null, cancellationToken))
            .WithMessage("A sales disposition reason with this code already exists.");
        RuleFor(x => x.Dto.DispositionReasonName).NotEmpty();
        RuleFor(x => x.Dto.DispositionReasonName).MaximumLength(100);
        RuleFor(x => x.Dto.DispositionReasonName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByDispositionReasonNameAsync(name, null, cancellationToken))
            .WithMessage("An active sales disposition reason with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);






        RuleFor(x => x.Dto).Must(dto => dto.AppliesToLead || dto.AppliesToOpportunity)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");
    }
}

public sealed class UpdateFgsSalesDispositionReasonCommandValidator : AbstractValidator<UpdateFgsSalesDispositionReasonCommand>
{
    public UpdateFgsSalesDispositionReasonCommandValidator(IFgsSalesDispositionReasonReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.DispositionReasonCode).NotEmpty();
        RuleFor(x => x.Dto.DispositionReasonCode).MaximumLength(50);
        RuleFor(x => x.Dto.DispositionReasonCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("DispositionReasonCode must be uppercase.");
        RuleFor(x => x.Dto.DispositionReasonCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByDispositionReasonCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A sales disposition reason with this code already exists.");
        RuleFor(x => x.Dto.DispositionReasonName).NotEmpty();
        RuleFor(x => x.Dto.DispositionReasonName).MaximumLength(100);
        RuleFor(x => x.Dto.DispositionReasonName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByDispositionReasonNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active sales disposition reason with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);






        RuleFor(x => x.Dto).Must(dto => dto.AppliesToLead || dto.AppliesToOpportunity)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");
    }
}

public sealed class PatchFgsSalesDispositionReasonCommandValidator : AbstractValidator<PatchFgsSalesDispositionReasonCommand>
{
    public PatchFgsSalesDispositionReasonCommandValidator(IFgsSalesDispositionReasonReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.DispositionReasonCode).NotEmpty();
        RuleFor(x => x.Dto.DispositionReasonCode).MaximumLength(50);
        RuleFor(x => x.Dto.DispositionReasonCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("DispositionReasonCode must be uppercase.");
        RuleFor(x => x.Dto.DispositionReasonCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByDispositionReasonCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A sales disposition reason with this code already exists.");
        RuleFor(x => x.Dto.DispositionReasonName).NotEmpty();
        RuleFor(x => x.Dto.DispositionReasonName).MaximumLength(100);
        RuleFor(x => x.Dto.DispositionReasonName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByDispositionReasonNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active sales disposition reason with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);






        RuleFor(x => x.Dto).Must(dto =>
                (!dto.AppliesToLead.HasValue && !dto.AppliesToOpportunity.HasValue)
                || dto.AppliesToLead == true
                || dto.AppliesToOpportunity == true)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");
    }
}
