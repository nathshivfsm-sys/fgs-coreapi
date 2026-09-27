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
        RuleFor(x => x.Dto.CategoryCode)
            .Must(v => !string.IsNullOrWhiteSpace(v))
            .WithMessage("CategoryCode must not be blank.");
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code, null, cancellationToken))
            .WithMessage("A job category with this code already exists.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.CategoryCode));
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name)
            .Must(v => !string.IsNullOrWhiteSpace(v))
            .WithMessage("Name must not be blank.");
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("A job category with this name already exists.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.Name));
        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
        RuleFor(x => x.Dto.BackgroundColor)
            .Matches(JobCategoryColorRules.HexPattern)
            .WithMessage(JobCategoryColorRules.HexMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.BackgroundColor));
        RuleFor(x => x.Dto.TextColor).MaximumLength(20);
        RuleFor(x => x.Dto.TextColor)
            .Matches(JobCategoryColorRules.HexPattern)
            .WithMessage(JobCategoryColorRules.HexMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.TextColor));
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class UpdateJobCategoryCommandValidator : AbstractValidator<UpdateJobCategoryCommand>
{
    public UpdateJobCategoryCommandValidator(IJobCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.CategoryCode).NotEmpty();
        RuleFor(x => x.Dto.CategoryCode)
            .Must(v => !string.IsNullOrWhiteSpace(v))
            .WithMessage("CategoryCode must not be blank.");
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.");
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A job category with this code already exists.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.CategoryCode));
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name)
            .Must(v => !string.IsNullOrWhiteSpace(v))
            .WithMessage("Name must not be blank.");
        RuleFor(x => x.Dto.Name).MaximumLength(150);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, command.Id, cancellationToken))
            .WithMessage("A job category with this name already exists.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.Name));
        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
        RuleFor(x => x.Dto.BackgroundColor)
            .Matches(JobCategoryColorRules.HexPattern)
            .WithMessage(JobCategoryColorRules.HexMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.BackgroundColor));
        RuleFor(x => x.Dto.TextColor).MaximumLength(20);
        RuleFor(x => x.Dto.TextColor)
            .Matches(JobCategoryColorRules.HexPattern)
            .WithMessage(JobCategoryColorRules.HexMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.TextColor));
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class PatchJobCategoryCommandValidator : AbstractValidator<PatchJobCategoryCommand>
{
    public PatchJobCategoryCommandValidator(IJobCategoryReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.CategoryCode).NotEmpty().When(x => x.Dto.CategoryCode is not null);
        RuleFor(x => x.Dto.CategoryCode)
            .Must(v => !string.IsNullOrWhiteSpace(v))
            .WithMessage("CategoryCode must not be blank.")
            .When(x => x.Dto.CategoryCode is not null);
        RuleFor(x => x.Dto.CategoryCode).MaximumLength(50).When(x => x.Dto.CategoryCode is not null);
        RuleFor(x => x.Dto.CategoryCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("CategoryCode must be uppercase.").When(x => x.Dto.CategoryCode is not null);
        RuleFor(x => x.Dto.CategoryCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCategoryCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A job category with this code already exists.").When(x => !string.IsNullOrWhiteSpace(x.Dto.CategoryCode));
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name)
            .Must(v => !string.IsNullOrWhiteSpace(v))
            .WithMessage("Name must not be blank.")
            .When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name).MaximumLength(150).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("A job category with this name already exists.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.Name));
        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20).When(x => x.Dto.BackgroundColor is not null);
        RuleFor(x => x.Dto.BackgroundColor)
            .Matches(JobCategoryColorRules.HexPattern)
            .WithMessage(JobCategoryColorRules.HexMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.BackgroundColor));
        RuleFor(x => x.Dto.TextColor).MaximumLength(20).When(x => x.Dto.TextColor is not null);
        RuleFor(x => x.Dto.TextColor)
            .Matches(JobCategoryColorRules.HexPattern)
            .WithMessage(JobCategoryColorRules.HexMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.TextColor));
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

internal static class JobCategoryColorRules
{
    public const string HexPattern = "^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$";
    public const string HexMessage = "Color must be a HEX value such as #FFF or #FFFFFF.";
}
