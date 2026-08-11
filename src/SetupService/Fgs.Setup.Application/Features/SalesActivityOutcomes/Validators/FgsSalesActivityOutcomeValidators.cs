using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.CreateFgsSalesActivityOutcome;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.PatchFgsSalesActivityOutcome;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.UpdateFgsSalesActivityOutcome;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Validators;

public sealed class CreateFgsSalesActivityOutcomeCommandValidator : AbstractValidator<CreateFgsSalesActivityOutcomeCommand>
{
    public CreateFgsSalesActivityOutcomeCommandValidator(IFgsSalesActivityOutcomeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.OutcomeCode).NotEmpty();
        RuleFor(x => x.Dto.OutcomeCode).MaximumLength(50);
        RuleFor(x => x.Dto.OutcomeCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("OutcomeCode must be uppercase.");
        RuleFor(x => x.Dto.OutcomeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByOutcomeCodeAsync(code, null, cancellationToken))
            .WithMessage("A sales activity outcome with this code already exists.");
        RuleFor(x => x.Dto.OutcomeName).NotEmpty();
        RuleFor(x => x.Dto.OutcomeName).MaximumLength(100);
        RuleFor(x => x.Dto.OutcomeName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByOutcomeNameAsync(name, null, cancellationToken))
            .WithMessage("An active sales activity outcome with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);



        RuleFor(x => x.Dto.NextSalesPipelineStatusId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsSalesPipelineStatusIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified sales pipeline status was not found.");



        RuleFor(x => x.Dto).Must(dto => dto.AppliesToLead || dto.AppliesToOpportunity)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");
    }
}

public sealed class UpdateFgsSalesActivityOutcomeCommandValidator : AbstractValidator<UpdateFgsSalesActivityOutcomeCommand>
{
    public UpdateFgsSalesActivityOutcomeCommandValidator(IFgsSalesActivityOutcomeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.OutcomeCode).NotEmpty();
        RuleFor(x => x.Dto.OutcomeCode).MaximumLength(50);
        RuleFor(x => x.Dto.OutcomeCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("OutcomeCode must be uppercase.");
        RuleFor(x => x.Dto.OutcomeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByOutcomeCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A sales activity outcome with this code already exists.");
        RuleFor(x => x.Dto.OutcomeName).NotEmpty();
        RuleFor(x => x.Dto.OutcomeName).MaximumLength(100);
        RuleFor(x => x.Dto.OutcomeName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByOutcomeNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active sales activity outcome with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);



        RuleFor(x => x.Dto.NextSalesPipelineStatusId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsSalesPipelineStatusIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified sales pipeline status was not found.");



        RuleFor(x => x.Dto).Must(dto => dto.AppliesToLead || dto.AppliesToOpportunity)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");
    }
}

public sealed class PatchFgsSalesActivityOutcomeCommandValidator : AbstractValidator<PatchFgsSalesActivityOutcomeCommand>
{
    public PatchFgsSalesActivityOutcomeCommandValidator(IFgsSalesActivityOutcomeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.OutcomeCode).NotEmpty().When(x => x.Dto.OutcomeCode is not null);
        RuleFor(x => x.Dto.OutcomeCode).MaximumLength(50).When(x => x.Dto.OutcomeCode is not null);
        RuleFor(x => x.Dto.OutcomeCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("OutcomeCode must be uppercase.").When(x => x.Dto.OutcomeCode is not null);
        RuleFor(x => x.Dto.OutcomeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByOutcomeCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A sales activity outcome with this code already exists.").When(x => x.Dto.OutcomeCode is not null);
        RuleFor(x => x.Dto.OutcomeName).NotEmpty().When(x => x.Dto.OutcomeName is not null);
        RuleFor(x => x.Dto.OutcomeName).MaximumLength(100).When(x => x.Dto.OutcomeName is not null);
        RuleFor(x => x.Dto.OutcomeName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByOutcomeNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active sales activity outcome with this name already exists.").When(x => x.Dto.OutcomeName is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);



        RuleFor(x => x.Dto.NextSalesPipelineStatusId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsSalesPipelineStatusIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified sales pipeline status was not found.").When(x => x.Dto.NextSalesPipelineStatusId.HasValue);



        RuleFor(x => x.Dto).Must(dto =>
                (!dto.AppliesToLead.HasValue && !dto.AppliesToOpportunity.HasValue)
                || dto.AppliesToLead == true
                || dto.AppliesToOpportunity == true)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");
    }
}
