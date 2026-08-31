using Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;
using Fgs.Setup.Application.Abstractions.TermsConditions;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.CreateFgsEntityDefaultTermsCondition;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.PatchFgsEntityDefaultTermsCondition;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.UpdateFgsEntityDefaultTermsCondition;
using FluentValidation;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Validators;

public sealed class CreateFgsEntityDefaultTermsConditionCommandValidator
    : AbstractValidator<CreateFgsEntityDefaultTermsConditionCommand>
{
    public CreateFgsEntityDefaultTermsConditionCommandValidator(
        IFgsEntityDefaultTermsConditionReadRepository readRepository,
        IFgsTermsConditionReadRepository termsConditionReadRepository)
    {
        RuleFor(x => x.Dto.EntityType).NotEmpty();
        RuleFor(x => x.Dto.TermsConditionId).GreaterThan(0);
        RuleFor(x => x.Dto.EntityType).MustAsync(async (command, entityType, cancellationToken) =>
                !await readRepository.ExistsByEntityTypeAsync(entityType, null, cancellationToken))
            .WithMessage("An active default terms condition for this entity type already exists.");
        RuleFor(x => x.Dto.TermsConditionId).MustAsync(async (command, termsConditionId, cancellationToken) =>
                await termsConditionReadRepository.ExistsByIdAsync(termsConditionId, cancellationToken))
            .WithMessage("The referenced terms condition was not found.");
    }
}

public sealed class UpdateFgsEntityDefaultTermsConditionCommandValidator
    : AbstractValidator<UpdateFgsEntityDefaultTermsConditionCommand>
{
    public UpdateFgsEntityDefaultTermsConditionCommandValidator(
        IFgsEntityDefaultTermsConditionReadRepository readRepository,
        IFgsTermsConditionReadRepository termsConditionReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.EntityType).NotEmpty();
        RuleFor(x => x.Dto.TermsConditionId).GreaterThan(0);
        RuleFor(x => x.Dto.EntityType).MustAsync(async (command, entityType, cancellationToken) =>
                !await readRepository.ExistsByEntityTypeAsync(entityType, command.Id, cancellationToken))
            .WithMessage("An active default terms condition for this entity type already exists.");
        RuleFor(x => x.Dto.TermsConditionId).MustAsync(async (command, termsConditionId, cancellationToken) =>
                await termsConditionReadRepository.ExistsByIdAsync(termsConditionId, cancellationToken))
            .WithMessage("The referenced terms condition was not found.");
    }
}

public sealed class PatchFgsEntityDefaultTermsConditionCommandValidator
    : AbstractValidator<PatchFgsEntityDefaultTermsConditionCommand>
{
    public PatchFgsEntityDefaultTermsConditionCommandValidator(
        IFgsEntityDefaultTermsConditionReadRepository readRepository,
        IFgsTermsConditionReadRepository termsConditionReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.EntityType).NotEmpty().When(x => x.Dto.EntityType is not null);
        RuleFor(x => x.Dto.TermsConditionId).GreaterThan(0).When(x => x.Dto.TermsConditionId.HasValue);
        RuleFor(x => x.Dto.EntityType).MustAsync(async (command, entityType, cancellationToken) =>
                !await readRepository.ExistsByEntityTypeAsync(entityType!, command.Id, cancellationToken))
            .WithMessage("An active default terms condition for this entity type already exists.")
            .When(x => x.Dto.EntityType is not null);
        RuleFor(x => x.Dto.TermsConditionId).MustAsync(async (command, termsConditionId, cancellationToken) =>
                await termsConditionReadRepository.ExistsByIdAsync(termsConditionId!.Value, cancellationToken))
            .WithMessage("The referenced terms condition was not found.")
            .When(x => x.Dto.TermsConditionId.HasValue);
    }
}
