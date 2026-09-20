using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.CreateFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.PatchFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.UpdateFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupTaxes.Validators;

public sealed class CreateFgsSetupTaxCommandValidator : AbstractValidator<CreateFgsSetupTaxCommand>
{
    public CreateFgsSetupTaxCommandValidator(
        IFgsSetupTaxReadRepository readRepository,
        IFgsSetupTaxAuthorityReadRepository taxAuthorityReadRepository)
    {
        RuleFor(x => x.Dto.TaxCode).NotEmpty();
        RuleFor(x => x.Dto.TaxCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("TaxCode must be uppercase.");
        RuleFor(x => x.Dto.TaxCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByTaxCodeAsync(code, null, cancellationToken))
            .WithMessage("A tax with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.ExternalSystemId).MaximumLength(200);
        RuleFor(x => x.Dto.SyncToken).MaximumLength(100);

        RuleFor(x => x.Dto.TaxDetails)
            .Must(HaveUniquePositiveIds)
            .WithMessage("TaxDetails must not contain duplicate positive Id values.")
            .When(x => x.Dto.TaxDetails is not null);

        RuleForEach(x => x.Dto.TaxDetails)
            .ChildRules(line =>
            {
                line.RuleFor(x => x.FgsSetupTaxAuthorityId).GreaterThan(0);
                line.RuleFor(x => x.FgsSetupTaxAuthorityId)
                    .MustAsync(async (authorityId, cancellationToken) =>
                        await taxAuthorityReadRepository.ExistsActiveByIdAsync(authorityId, cancellationToken))
                    .WithMessage("The specified tax authority was not found or is inactive.");
                line.RuleFor(x => x.EffectiveToDate)
                    .GreaterThanOrEqualTo(x => x.EffectiveFromDate)
                    .When(x => x.EffectiveToDate.HasValue)
                    .WithMessage("EffectiveToDate must be greater than or equal to EffectiveFromDate.");
            })
            .When(x => x.Dto.TaxDetails is not null);
    }

    private static bool HaveUniquePositiveIds(IReadOnlyList<FgsSetupTaxLineUpsertDto>? lines)
    {
        if (lines is null)
        {
            return true;
        }

        var positiveIds = lines.Where(l => l.Id is > 0).Select(l => l.Id!.Value).ToList();
        return positiveIds.Count == positiveIds.Distinct().Count();
    }
}

public sealed class UpdateFgsSetupTaxCommandValidator : AbstractValidator<UpdateFgsSetupTaxCommand>
{
    public UpdateFgsSetupTaxCommandValidator(
        IFgsSetupTaxReadRepository readRepository,
        IFgsSetupTaxAuthorityReadRepository taxAuthorityReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.TaxCode).NotEmpty();
        RuleFor(x => x.Dto.TaxCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("TaxCode must be uppercase.");
        RuleFor(x => x.Dto.TaxCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByTaxCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A tax with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.ExternalSystemId).MaximumLength(200);
        RuleFor(x => x.Dto.SyncToken).MaximumLength(100);

        RuleFor(x => x.Dto.TaxDetails)
            .Must(HaveUniquePositiveIds)
            .WithMessage("TaxDetails must not contain duplicate positive Id values.")
            .When(x => x.Dto.TaxDetails is not null);

        RuleForEach(x => x.Dto.TaxDetails)
            .ChildRules(line =>
            {
                line.RuleFor(x => x.FgsSetupTaxAuthorityId).GreaterThan(0);
                line.RuleFor(x => x.FgsSetupTaxAuthorityId)
                    .MustAsync(async (authorityId, cancellationToken) =>
                        await taxAuthorityReadRepository.ExistsActiveByIdAsync(authorityId, cancellationToken))
                    .WithMessage("The specified tax authority was not found or is inactive.");
                line.RuleFor(x => x.EffectiveToDate)
                    .GreaterThanOrEqualTo(x => x.EffectiveFromDate)
                    .When(x => x.EffectiveToDate.HasValue)
                    .WithMessage("EffectiveToDate must be greater than or equal to EffectiveFromDate.");
            })
            .When(x => x.Dto.TaxDetails is not null);
    }

    private static bool HaveUniquePositiveIds(IReadOnlyList<FgsSetupTaxLineUpsertDto>? lines)
    {
        if (lines is null)
        {
            return true;
        }

        var positiveIds = lines.Where(l => l.Id is > 0).Select(l => l.Id!.Value).ToList();
        return positiveIds.Count == positiveIds.Distinct().Count();
    }
}

public sealed class PatchFgsSetupTaxCommandValidator : AbstractValidator<PatchFgsSetupTaxCommand>
{
    public PatchFgsSetupTaxCommandValidator(IFgsSetupTaxReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.TaxCode).NotEmpty().When(x => x.Dto.TaxCode is not null);
        RuleFor(x => x.Dto.TaxCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("TaxCode must be uppercase.").When(x => x.Dto.TaxCode is not null);
        RuleFor(x => x.Dto.TaxCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByTaxCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A tax with this code already exists.").When(x => x.Dto.TaxCode is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.ExternalSystemId).MaximumLength(200).When(x => x.Dto.ExternalSystemId is not null);
        RuleFor(x => x.Dto.SyncToken).MaximumLength(100).When(x => x.Dto.SyncToken is not null);
    }
}
