using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Commands.CreateJobTypeCategory;
using Fgs.Setup.Application.Features.JobTypeCategories.Commands.PatchJobTypeCategory;
using Fgs.Setup.Application.Features.JobTypeCategories.Commands.UpdateJobTypeCategory;
using FluentValidation;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Validators;

public sealed class CreateJobTypeCategoryCommandValidator : AbstractValidator<CreateJobTypeCategoryCommand>
{
    public CreateJobTypeCategoryCommandValidator(IJobTypeCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByJobTypeIdAndJobCategoryIdAsync(dto.JobTypeId, dto.JobCategoryId, null, cancellationToken))
            .WithMessage("A job type category with this combination already exists.");
        RuleFor(x => x.Dto.JobTypeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsJobTypeIdAsync(value, cancellationToken))
            .WithMessage("The specified job type was not found.");
        RuleFor(x => x.Dto.JobCategoryId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsJobCategoryIdAsync(value, cancellationToken))
            .WithMessage("The specified job category was not found.");
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class UpdateJobTypeCategoryCommandValidator : AbstractValidator<UpdateJobTypeCategoryCommand>
{
    public UpdateJobTypeCategoryCommandValidator(IJobTypeCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByJobTypeIdAndJobCategoryIdAsync(dto.JobTypeId, dto.JobCategoryId, command.Id, cancellationToken))
            .WithMessage("A job type category with this combination already exists.");
        RuleFor(x => x.Dto.JobTypeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsJobTypeIdAsync(value, cancellationToken))
            .WithMessage("The specified job type was not found.");
        RuleFor(x => x.Dto.JobCategoryId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsJobCategoryIdAsync(value, cancellationToken))
            .WithMessage("The specified job category was not found.");
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class PatchJobTypeCategoryCommandValidator : AbstractValidator<PatchJobTypeCategoryCommand>
{
    public PatchJobTypeCategoryCommandValidator(IJobTypeCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !dto.JobTypeId.HasValue
                || !dto.JobCategoryId.HasValue
                || !await readRepository.ExistsByJobTypeIdAndJobCategoryIdAsync(
                    dto.JobTypeId.Value,
                    dto.JobCategoryId.Value,
                    command.Id,
                    cancellationToken))
            .WithMessage("A job type category with this combination already exists.");
        RuleFor(x => x.Dto.JobTypeId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsJobTypeIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified job type was not found.").When(x => x.Dto.JobTypeId.HasValue);
        RuleFor(x => x.Dto.JobCategoryId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsJobCategoryIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified job category was not found.").When(x => x.Dto.JobCategoryId.HasValue);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
