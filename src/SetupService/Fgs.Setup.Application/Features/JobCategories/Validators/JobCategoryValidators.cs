using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Commands.CreateJobCategory;
using Fgs.Setup.Application.Features.JobCategories.Commands.PatchJobCategory;
using Fgs.Setup.Application.Features.JobCategories.Commands.UpdateJobCategory;
using FluentValidation;

namespace Fgs.Setup.Application.Features.JobCategories.Validators;

public sealed class CreateJobCategoryCommandValidator : AbstractValidator<CreateJobCategoryCommand>
{
    public CreateJobCategoryCommandValidator(IJobCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Dto.CategoryCode).NotEmpty();
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code, null, cancellationToken))
            .WithMessage("A job category with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);        RuleFor(x => x.Dto.CategoryCode).NotEmpty();
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code, null, cancellationToken))
            .WithMessage("A job category with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class UpdateJobCategoryCommandValidator : AbstractValidator<UpdateJobCategoryCommand>
{
    public UpdateJobCategoryCommandValidator(IJobCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.CategoryCode).NotEmpty();
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A job category with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class PatchJobCategoryCommandValidator : AbstractValidator<PatchJobCategoryCommand>
{
    public PatchJobCategoryCommandValidator(IJobCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.CategoryCode).NotEmpty();
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A job category with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
