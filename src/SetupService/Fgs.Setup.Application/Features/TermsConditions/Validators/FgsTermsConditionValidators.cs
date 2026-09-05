using Fgs.Setup.Application.Abstractions.TermsConditions;
using Fgs.Setup.Application.Features.TermsConditions.Commands.CreateFgsTermsCondition;
using Fgs.Setup.Application.Features.TermsConditions.Commands.PatchFgsTermsCondition;
using Fgs.Setup.Application.Features.TermsConditions.Commands.UpdateFgsTermsCondition;
using FluentValidation;

namespace Fgs.Setup.Application.Features.TermsConditions.Validators;

public sealed class CreateFgsTermsConditionCommandValidator : AbstractValidator<CreateFgsTermsConditionCommand>
{
    public CreateFgsTermsConditionCommandValidator(IFgsTermsConditionReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.TermsText).NotEmpty();
        RuleFor(x => x.Dto.VersionNumber).GreaterThan(0);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByCodeAndVersionAsync(dto.Code, dto.VersionNumber, null, cancellationToken))
            .WithMessage("An active terms condition with this code and version already exists.");
    }
}

public sealed class UpdateFgsTermsConditionCommandValidator : AbstractValidator<UpdateFgsTermsConditionCommand>
{
    public UpdateFgsTermsConditionCommandValidator(IFgsTermsConditionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.TermsText).NotEmpty();
        RuleFor(x => x.Dto.VersionNumber).GreaterThan(0);
        RuleFor(x => x).MustAsync(async (command, cancellationToken) =>
                !await readRepository.ExistsByCodeAndVersionAsync(
                    command.Dto.Code,
                    command.Dto.VersionNumber,
                    command.Id,
                    cancellationToken))
            .WithMessage("An active terms condition with this code and version already exists.");
    }
}

public sealed class PatchFgsTermsConditionCommandValidator : AbstractValidator<PatchFgsTermsConditionCommand>
{
    public PatchFgsTermsConditionCommandValidator(IFgsTermsConditionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.TermsText).NotEmpty().When(x => x.Dto.TermsText is not null);
        RuleFor(x => x.Dto.VersionNumber).GreaterThan(0).When(x => x.Dto.VersionNumber.HasValue);
        RuleFor(x => x).MustAsync(async (command, cancellationToken) =>
            {
                if (command.Dto.Code is null && command.Dto.VersionNumber is null)
                {
                    return true;
                }

                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                var code = command.Dto.Code ?? existing.Code;
                var version = command.Dto.VersionNumber ?? existing.VersionNumber;
                return !await readRepository.ExistsByCodeAndVersionAsync(code, version, command.Id, cancellationToken);
            })
            .WithMessage("An active terms condition with this code and version already exists.")
            .When(x => x.Dto.Code is not null || x.Dto.VersionNumber.HasValue);
    }
}
