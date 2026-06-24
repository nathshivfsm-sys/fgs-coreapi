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
        RuleFor(x => x.Dto.CategoryCode).NotEmpty();
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code, null, cancellationToken))
            .WithMessage("A job type category with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);        RuleFor(x => x.Dto.CategoryCode).NotEmpty();
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code, null, cancellationToken))
            .WithMessage("A job type category with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class UpdateJobTypeCategoryCommandValidator : AbstractValidator<UpdateJobTypeCategoryCommand>
{
    public UpdateJobTypeCategoryCommandValidator(IJobTypeCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.CategoryCode).NotEmpty();
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A job type category with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class PatchJobTypeCategoryCommandValidator : AbstractValidator<PatchJobTypeCategoryCommand>
{
    public PatchJobTypeCategoryCommandValidator(IJobTypeCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.CategoryCode).NotEmpty();
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A job type category with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(150);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
