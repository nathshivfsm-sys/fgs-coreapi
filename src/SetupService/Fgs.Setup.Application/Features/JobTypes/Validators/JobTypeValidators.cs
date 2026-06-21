using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Commands.CreateJobType;
using Fgs.Setup.Application.Features.JobTypes.Commands.PatchJobType;
using Fgs.Setup.Application.Features.JobTypes.Commands.UpdateJobType;
using FluentValidation;

namespace Fgs.Setup.Application.Features.JobTypes.Validators;

public sealed class CreateJobTypeCommandValidator : AbstractValidator<CreateJobTypeCommand>
{
    public CreateJobTypeCommandValidator(IJobTypeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.JobTypeCategoryId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsJobTypeCategoryIdAsync(value, cancellationToken))
            .WithMessage("The specified job type category was not found.");
        RuleFor(x => x.Dto.JobTypeSubCategoryId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsJobTypeSubCategoryIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified job type subcategory was not found.");
        RuleFor(x => x.Dto.JobTypeCode).NotEmpty();
        RuleFor(x => x.Dto.JobTypeCode).MaximumLength(50);
        RuleFor(x => x.Dto.JobTypeCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("JobTypeCode must be uppercase.");
        RuleFor(x => x.Dto.JobTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByJobTypeCodeAsync(code, null, cancellationToken))
            .WithMessage("A job type with this code already exists.");
        RuleFor(x => x.Dto.TaskName).NotEmpty();
        RuleFor(x => x.Dto.TaskName).MaximumLength(200);

        RuleFor(x => x.Dto.UsedFor).NotEmpty();
        RuleFor(x => x.Dto.UsedFor).MaximumLength(50);
        RuleFor(x => x.Dto.Trade).MaximumLength(100);

        RuleFor(x => x.Dto.BusinessUnit).MaximumLength(100);
        RuleFor(x => x.Dto.Priority).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20);


        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class UpdateJobTypeCommandValidator : AbstractValidator<UpdateJobTypeCommand>
{
    public UpdateJobTypeCommandValidator(IJobTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.JobTypeCategoryId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsJobTypeCategoryIdAsync(value, cancellationToken))
            .WithMessage("The specified job type category was not found.");
        RuleFor(x => x.Dto.JobTypeSubCategoryId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsJobTypeSubCategoryIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified job type subcategory was not found.");
        RuleFor(x => x.Dto.JobTypeCode).NotEmpty();
        RuleFor(x => x.Dto.JobTypeCode).MaximumLength(50);
        RuleFor(x => x.Dto.JobTypeCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("JobTypeCode must be uppercase.");
        RuleFor(x => x.Dto.JobTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByJobTypeCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A job type with this code already exists.");
        RuleFor(x => x.Dto.TaskName).NotEmpty();
        RuleFor(x => x.Dto.TaskName).MaximumLength(200);

        RuleFor(x => x.Dto.UsedFor).NotEmpty();
        RuleFor(x => x.Dto.UsedFor).MaximumLength(50);
        RuleFor(x => x.Dto.Trade).MaximumLength(100);

        RuleFor(x => x.Dto.BusinessUnit).MaximumLength(100);
        RuleFor(x => x.Dto.Priority).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20);


        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class PatchJobTypeCommandValidator : AbstractValidator<PatchJobTypeCommand>
{
    public PatchJobTypeCommandValidator(IJobTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.JobTypeCategoryId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsJobTypeCategoryIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified job type category was not found.").When(x => x.Dto.JobTypeCategoryId.HasValue);
        RuleFor(x => x.Dto.JobTypeSubCategoryId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsJobTypeSubCategoryIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified job type subcategory was not found.").When(x => x.Dto.JobTypeSubCategoryId.HasValue);
        RuleFor(x => x.Dto.JobTypeCode).NotEmpty();
        RuleFor(x => x.Dto.JobTypeCode).MaximumLength(50);
        RuleFor(x => x.Dto.JobTypeCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("JobTypeCode must be uppercase.");
        RuleFor(x => x.Dto.JobTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByJobTypeCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A job type with this code already exists.");
        RuleFor(x => x.Dto.TaskName).NotEmpty();
        RuleFor(x => x.Dto.TaskName).MaximumLength(200);

        RuleFor(x => x.Dto.UsedFor).NotEmpty();
        RuleFor(x => x.Dto.UsedFor).MaximumLength(50);
        RuleFor(x => x.Dto.Trade).MaximumLength(100).When(x => x.Dto.Trade is not null);

        RuleFor(x => x.Dto.BusinessUnit).MaximumLength(100).When(x => x.Dto.BusinessUnit is not null);
        RuleFor(x => x.Dto.Priority).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20).When(x => x.Dto.BackgroundColor is not null);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20).When(x => x.Dto.TextColor is not null);


        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
