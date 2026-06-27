using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.CreateJobTypeSubCategory;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.PatchJobTypeSubCategory;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.UpdateJobTypeSubCategory;
using FluentValidation;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Validators;

public sealed class CreateJobTypeSubCategoryCommandValidator : AbstractValidator<CreateJobTypeSubCategoryCommand>
{
    public CreateJobTypeSubCategoryCommandValidator(IJobTypeSubCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Dto.SubCategoryCode).NotEmpty();
        RuleFor(x => x.Dto.SubCategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.SubCategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SubCategoryCode must be uppercase.");
        RuleFor(x => x.Dto.SubCategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySubCategoryCodeAsync(code, null, cancellationToken))
            .WithMessage("A job type subcategory with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue); RuleFor(x => x.Dto.SubCategoryCode).NotEmpty();
        RuleFor(x => x.Dto.SubCategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.SubCategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SubCategoryCode must be uppercase.");
        RuleFor(x => x.Dto.SubCategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySubCategoryCodeAsync(code, null, cancellationToken))
            .WithMessage("A job type subcategory with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class UpdateJobTypeSubCategoryCommandValidator : AbstractValidator<UpdateJobTypeSubCategoryCommand>
{
    public UpdateJobTypeSubCategoryCommandValidator(IJobTypeSubCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.SubCategoryCode).NotEmpty();
        RuleFor(x => x.Dto.SubCategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.SubCategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SubCategoryCode must be uppercase.");
        RuleFor(x => x.Dto.SubCategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySubCategoryCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A job type subcategory with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class PatchJobTypeSubCategoryCommandValidator : AbstractValidator<PatchJobTypeSubCategoryCommand>
{
    public PatchJobTypeSubCategoryCommandValidator(IJobTypeSubCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.SubCategoryCode).NotEmpty();
        RuleFor(x => x.Dto.SubCategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.SubCategoryCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SubCategoryCode must be uppercase.");
        RuleFor(x => x.Dto.SubCategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySubCategoryCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A job type subcategory with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
