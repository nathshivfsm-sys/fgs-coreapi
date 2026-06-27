using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.CreateTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.PatchTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.UpdateTitleOfCourtesy;
using FluentValidation;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Validators;

public sealed class CreateTitleOfCourtesyCommandValidator : AbstractValidator<CreateTitleOfCourtesyCommand>
{
    public CreateTitleOfCourtesyCommandValidator(ITitleOfCourtesyReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Code must be uppercase.")
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A title of courtesy with this code already exists.");

        RuleFor(x => x.Dto.DisplayName)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(async (command, displayName, cancellationToken) =>
                !await readRepository.ExistsByDisplayNameAsync(displayName, null, cancellationToken))
            .WithMessage("An active title of courtesy with this display name already exists.");

        RuleFor(x => x.Dto.SortOrder)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Dto.SortOrder.HasValue);
    }
}

public sealed class UpdateTitleOfCourtesyCommandValidator : AbstractValidator<UpdateTitleOfCourtesyCommand>
{
    public UpdateTitleOfCourtesyCommandValidator(ITitleOfCourtesyReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Code must be uppercase.")
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A title of courtesy with this code already exists.");

        RuleFor(x => x.Dto.DisplayName)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(async (command, displayName, cancellationToken) =>
                !await readRepository.ExistsByDisplayNameAsync(displayName, command.Id, cancellationToken))
            .WithMessage("An active title of courtesy with this display name already exists.");

        RuleFor(x => x.Dto.SortOrder)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Dto.SortOrder.HasValue);
    }
}

public sealed class PatchTitleOfCourtesyCommandValidator : AbstractValidator<PatchTitleOfCourtesyCommand>
{
    public PatchTitleOfCourtesyCommandValidator(ITitleOfCourtesyReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Code must be uppercase.")
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A title of courtesy with this code already exists.")
            .When(x => x.Dto.Code is not null);

        RuleFor(x => x.Dto.DisplayName)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(async (command, displayName, cancellationToken) =>
                !await readRepository.ExistsByDisplayNameAsync(displayName!, command.Id, cancellationToken))
            .WithMessage("An active title of courtesy with this display name already exists.")
            .When(x => x.Dto.DisplayName is not null);

        RuleFor(x => x.Dto.SortOrder)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Dto.SortOrder.HasValue);
    }
}
