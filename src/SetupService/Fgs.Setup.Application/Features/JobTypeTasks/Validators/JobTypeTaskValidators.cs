using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.CreateJobTypeTask;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.PatchJobTypeTask;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.UpdateJobTypeTask;
using FluentValidation;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Validators;

public sealed class CreateJobTypeTaskCommandValidator : AbstractValidator<CreateJobTypeTaskCommand>
{
    public CreateJobTypeTaskCommandValidator(IJobTypeTaskReadRepository readRepository)
    {
        RuleFor(x => x.Dto.JobTypeCategoryId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsJobTypeCategoryIdAsync(value, cancellationToken))
            .WithMessage("The specified job type category was not found.");
        RuleFor(x => x.Dto.TradeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsTradeIdAsync(value, cancellationToken))
            .WithMessage("The specified trade was not found.");
        RuleFor(x => x.Dto.TaskName).NotEmpty();
        RuleFor(x => x.Dto.TaskName).MaximumLength(200);
        RuleFor(x => x.Dto.Priority).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.EstimatedHours).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);        RuleFor(x => x.Dto.JobTypeCategoryId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsJobTypeCategoryIdAsync(value, cancellationToken))
            .WithMessage("The specified job type category was not found.");
        RuleFor(x => x.Dto.TradeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsTradeIdAsync(value, cancellationToken))
            .WithMessage("The specified trade was not found.");
        RuleFor(x => x.Dto.TaskName).NotEmpty();
        RuleFor(x => x.Dto.TaskName).MaximumLength(200);
        RuleFor(x => x.Dto.Priority).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.EstimatedHours).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class UpdateJobTypeTaskCommandValidator : AbstractValidator<UpdateJobTypeTaskCommand>
{
    public UpdateJobTypeTaskCommandValidator(IJobTypeTaskReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.JobTypeCategoryId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsJobTypeCategoryIdAsync(value, cancellationToken))
            .WithMessage("The specified job type category was not found.");
        RuleFor(x => x.Dto.TradeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsTradeIdAsync(value, cancellationToken))
            .WithMessage("The specified trade was not found.");
        RuleFor(x => x.Dto.TaskName).NotEmpty();
        RuleFor(x => x.Dto.TaskName).MaximumLength(200);
        RuleFor(x => x.Dto.Priority).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.EstimatedHours).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class PatchJobTypeTaskCommandValidator : AbstractValidator<PatchJobTypeTaskCommand>
{
    public PatchJobTypeTaskCommandValidator(IJobTypeTaskReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.JobTypeCategoryId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsJobTypeCategoryIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified job type category was not found.").When(x => x.Dto.JobTypeCategoryId.HasValue);
        RuleFor(x => x.Dto.TradeId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTradeIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified trade was not found.").When(x => x.Dto.TradeId.HasValue);
        RuleFor(x => x.Dto.TaskName).NotEmpty().When(x => x.Dto.TaskName is not null);
        RuleFor(x => x.Dto.TaskName).MaximumLength(200).When(x => x.Dto.TaskName is not null);
        RuleFor(x => x.Dto.Priority).GreaterThanOrEqualTo((short)1).When(x => x.Dto.Priority.HasValue);
        RuleFor(x => x.Dto.EstimatedHours).GreaterThanOrEqualTo(0m).When(x => x.Dto.EstimatedHours.HasValue);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
