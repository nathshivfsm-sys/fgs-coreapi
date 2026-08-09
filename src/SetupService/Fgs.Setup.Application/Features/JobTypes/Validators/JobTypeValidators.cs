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
        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20);


        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
        RuleFor(x => x.Dto.UsedFor).InclusiveBetween((short)1, (short)4);
    }
}

public sealed class UpdateJobTypeCommandValidator : AbstractValidator<UpdateJobTypeCommand>
{
    public UpdateJobTypeCommandValidator(IJobTypeReadRepository readRepository)
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
        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20);


        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
        RuleFor(x => x.Dto.UsedFor).InclusiveBetween((short)1, (short)4);
    }
}

public sealed class PatchJobTypeCommandValidator : AbstractValidator<PatchJobTypeCommand>
{
    public PatchJobTypeCommandValidator(IJobTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.JobTypeCode).NotEmpty();
        RuleFor(x => x.Dto.JobTypeCode).MaximumLength(50);
        RuleFor(x => x.Dto.JobTypeCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("JobTypeCode must be uppercase.");
        RuleFor(x => x.Dto.JobTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByJobTypeCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A job type with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active job type with this name already exists.");
        RuleFor(x => x.Dto.UsedFor).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.BusinessUnit).MaximumLength(100).When(x => x.Dto.BusinessUnit is not null);
        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20).When(x => x.Dto.BackgroundColor is not null);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20).When(x => x.Dto.TextColor is not null);


        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
        RuleFor(x => x.Dto.UsedFor).InclusiveBetween((short)1, (short)4).When(x => x.Dto.UsedFor.HasValue);
    }
}
