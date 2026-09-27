using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Commands.CreateJobType;
using Fgs.Setup.Application.Features.JobTypes.Commands.PatchJobType;
using Fgs.Setup.Application.Features.JobTypes.Commands.UpdateJobType;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using FluentValidation;

namespace Fgs.Setup.Application.Features.JobTypes.Validators;

public sealed class CreateJobTypeCommandValidator : AbstractValidator<CreateJobTypeCommand>
{
    public CreateJobTypeCommandValidator(
        IJobTypeReadRepository readRepository,
        IJobTypeTaskReadRepository taskReadRepository)
    {
        RuleFor(x => x.Dto.JobTypeCode).NotEmpty();
        RuleFor(x => x.Dto.JobTypeCode).MaximumLength(50);
        RuleFor(x => x.Dto.JobTypeCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("JobTypeCode must be uppercase.");
        RuleFor(x => x.Dto.JobTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByJobTypeCodeAsync(code, null, cancellationToken))
            .WithMessage("A job type with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("An active job type with this name already exists.");
        RuleFor(x => x.Dto.UsedFor).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.BusinessUnit).MaximumLength(100);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
        RuleFor(x => x.Dto.UsedFor).InclusiveBetween((short)1, (short)4);
        AddRequiredSubCategoryRules(this, taskReadRepository);
    }

    private static void AddRequiredSubCategoryRules(
        AbstractValidator<CreateJobTypeCommand> validator,
        IJobTypeTaskReadRepository taskReadRepository)
    {
        validator.RuleFor(x => x.Dto.SubCategories)
            .NotEmpty()
            .WithMessage("At least one subcategory is required.");
        validator.RuleFor(x => x.Dto.SubCategories)
            .Must(HaveUniqueTaskIds)
            .WithMessage("SubCategories cannot contain duplicate job type tasks.")
            .When(x => x.Dto.SubCategories is not null);
        validator.RuleForEach(x => x.Dto.SubCategories)
            .SetValidator(new JobTypeSubCategoryWriteDtoValidator(taskReadRepository))
            .When(x => x.Dto.SubCategories is not null);
    }

    private static bool HaveUniqueTaskIds(IReadOnlyList<JobTypeSubCategoryWriteDto>? items) =>
        items is null || items.Select(item => item.JobTypeTaskId).Distinct().Count() == items.Count;
}

public sealed class UpdateJobTypeCommandValidator : AbstractValidator<UpdateJobTypeCommand>
{
    public UpdateJobTypeCommandValidator(
        IJobTypeReadRepository readRepository,
        IJobTypeTaskReadRepository taskReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.JobTypeCode).NotEmpty();
        RuleFor(x => x.Dto.JobTypeCode).MaximumLength(50);
        RuleFor(x => x.Dto.JobTypeCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("JobTypeCode must be uppercase.");
        RuleFor(x => x.Dto.JobTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByJobTypeCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A job type with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active job type with this name already exists.");
        RuleFor(x => x.Dto.UsedFor).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.BusinessUnit).MaximumLength(100);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
        RuleFor(x => x.Dto.UsedFor).InclusiveBetween((short)1, (short)4);
        RuleFor(x => x.Dto.SubCategories)
            .NotEmpty()
            .WithMessage("At least one subcategory is required.");
        RuleFor(x => x.Dto.SubCategories)
            .Must(items => items is null || items.Select(item => item.JobTypeTaskId).Distinct().Count() == items.Count)
            .WithMessage("SubCategories cannot contain duplicate job type tasks.")
            .When(x => x.Dto.SubCategories is not null);
        RuleForEach(x => x.Dto.SubCategories)
            .SetValidator(new JobTypeSubCategoryWriteDtoValidator(taskReadRepository))
            .When(x => x.Dto.SubCategories is not null);
    }
}

public sealed class PatchJobTypeCommandValidator : AbstractValidator<PatchJobTypeCommand>
{
    public PatchJobTypeCommandValidator(
        IJobTypeReadRepository readRepository,
        IJobTypeTaskReadRepository taskReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.JobTypeCode).NotEmpty().When(x => x.Dto.JobTypeCode is not null);
        RuleFor(x => x.Dto.JobTypeCode).MaximumLength(50).When(x => x.Dto.JobTypeCode is not null);
        RuleFor(x => x.Dto.JobTypeCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("JobTypeCode must be uppercase.").When(x => x.Dto.JobTypeCode is not null);
        RuleFor(x => x.Dto.JobTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByJobTypeCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A job type with this code already exists.").When(x => x.Dto.JobTypeCode is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name).MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active job type with this name already exists.").When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.UsedFor).GreaterThanOrEqualTo((short)1).When(x => x.Dto.UsedFor.HasValue);
        RuleFor(x => x.Dto.BusinessUnit).MaximumLength(100).When(x => x.Dto.BusinessUnit is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
        RuleFor(x => x.Dto.UsedFor).InclusiveBetween((short)1, (short)4).When(x => x.Dto.UsedFor.HasValue);
        When(x => x.Dto.SubCategories is not null, () =>
        {
            RuleFor(x => x.Dto.SubCategories)
                .NotEmpty()
                .WithMessage("At least one subcategory is required.");
            RuleFor(x => x.Dto.SubCategories)
                .Must(items => items is null || items.Select(item => item.JobTypeTaskId).Distinct().Count() == items.Count)
                .WithMessage("SubCategories cannot contain duplicate job type tasks.");
            RuleForEach(x => x.Dto.SubCategories)
                .SetValidator(new JobTypeSubCategoryWriteDtoValidator(taskReadRepository));
        });
    }
}

internal sealed class JobTypeSubCategoryWriteDtoValidator : AbstractValidator<JobTypeSubCategoryWriteDto>
{
    public JobTypeSubCategoryWriteDtoValidator(IJobTypeTaskReadRepository taskReadRepository)
    {
        RuleFor(x => x.JobTypeTaskId).GreaterThan(0);
        RuleFor(x => x.JobTypeTaskId)
            .MustAsync(async (id, cancellationToken) =>
                await taskReadRepository.ExistsActiveByIdAsync(id, cancellationToken))
            .WithMessage("The specified job type task was not found.");
        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo((short)0)
            .When(x => x.DisplayOrder.HasValue);
    }
}
