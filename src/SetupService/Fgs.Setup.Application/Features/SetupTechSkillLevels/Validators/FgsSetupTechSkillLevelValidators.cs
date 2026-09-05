using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.CreateFgsSetupTechSkillLevel;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.PatchFgsSetupTechSkillLevel;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.UpdateFgsSetupTechSkillLevel;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Validators;

public sealed class CreateFgsSetupTechSkillLevelCommandValidator : AbstractValidator<CreateFgsSetupTechSkillLevelCommand>
{
    public CreateFgsSetupTechSkillLevelCommandValidator(IFgsSetupTechSkillLevelReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A tech skill level with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0).When(x => x.Dto.SortOrder.HasValue); RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A tech skill level with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0).When(x => x.Dto.SortOrder.HasValue);
    }
}

public sealed class UpdateFgsSetupTechSkillLevelCommandValidator : AbstractValidator<UpdateFgsSetupTechSkillLevelCommand>
{
    public UpdateFgsSetupTechSkillLevelCommandValidator(IFgsSetupTechSkillLevelReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A tech skill level with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0).When(x => x.Dto.SortOrder.HasValue);
    }
}

public sealed class PatchFgsSetupTechSkillLevelCommandValidator : AbstractValidator<PatchFgsSetupTechSkillLevelCommand>
{
    public PatchFgsSetupTechSkillLevelCommandValidator(IFgsSetupTechSkillLevelReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code).MaximumLength(100).When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.").When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A tech skill level with this code already exists.").When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0).When(x => x.Dto.SortOrder.HasValue);
    }
}
