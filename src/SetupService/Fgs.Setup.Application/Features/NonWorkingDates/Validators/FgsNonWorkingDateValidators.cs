using Fgs.Setup.Application.Abstractions.NonWorkingDates;
using Fgs.Setup.Application.Features.NonWorkingDates.Commands.CreateFgsNonWorkingDate;
using Fgs.Setup.Application.Features.NonWorkingDates.Commands.PatchFgsNonWorkingDate;
using Fgs.Setup.Application.Features.NonWorkingDates.Commands.UpdateFgsNonWorkingDate;
using FluentValidation;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Validators;

public sealed class CreateFgsNonWorkingDateCommandValidator : AbstractValidator<CreateFgsNonWorkingDateCommand>
{
    public CreateFgsNonWorkingDateCommandValidator(IFgsNonWorkingDateReadRepository readRepository)
    {
        RuleFor(x => x.Dto.NonWorkingDate).NotEmpty();
        RuleFor(x => x.Dto.NonWorkingDate)
            .MustAsync(async (command, date, cancellationToken) =>
                !await readRepository.ExistsByNonWorkingDateAsync(date, null, cancellationToken))
            .WithMessage("A non-working date with this calendar date already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
    }
}

public sealed class UpdateFgsNonWorkingDateCommandValidator : AbstractValidator<UpdateFgsNonWorkingDateCommand>
{
    public UpdateFgsNonWorkingDateCommandValidator(IFgsNonWorkingDateReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.NonWorkingDate).NotEmpty();
        RuleFor(x => x.Dto.NonWorkingDate)
            .MustAsync(async (command, date, cancellationToken) =>
                !await readRepository.ExistsByNonWorkingDateAsync(date, command.Id, cancellationToken))
            .WithMessage("A non-working date with this calendar date already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
    }
}

public sealed class PatchFgsNonWorkingDateCommandValidator : AbstractValidator<PatchFgsNonWorkingDateCommand>
{
    public PatchFgsNonWorkingDateCommandValidator(IFgsNonWorkingDateReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.NonWorkingDate)
            .MustAsync(async (command, date, cancellationToken) =>
                !await readRepository.ExistsByNonWorkingDateAsync(date!.Value, command.Id, cancellationToken))
            .WithMessage("A non-working date with this calendar date already exists.")
            .When(x => x.Dto.NonWorkingDate.HasValue);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100).When(x => x.Dto.Name is not null);
    }
}
