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
        RuleForEach(x => x.Dto.TaxDetails).SetValidator(new FgsSetupTaxAuthorityAssignmentWriteDtoValidator(taxAuthorityReadRepository));
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
        RuleForEach(x => x.Dto.TaxDetails).SetValidator(new FgsSetupTaxAuthorityAssignmentWriteDtoValidator(taxAuthorityReadRepository));
    }
}

public sealed class PatchFgsSetupTaxCommandValidator : AbstractValidator<PatchFgsSetupTaxCommand>
{
    public PatchFgsSetupTaxCommandValidator(
        IFgsSetupTaxReadRepository readRepository,
        IFgsSetupTaxAuthorityReadRepository taxAuthorityReadRepository)
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
        RuleForEach(x => x.Dto.TaxDetails!).SetValidator(new FgsSetupTaxAuthorityAssignmentWriteDtoValidator(taxAuthorityReadRepository)).When(x => x.Dto.TaxDetails is not null);
    }
}

internal sealed class FgsSetupTaxAuthorityAssignmentWriteDtoValidator : AbstractValidator<FgsSetupTaxAuthorityAssignmentWriteDto>
{
    public FgsSetupTaxAuthorityAssignmentWriteDtoValidator(IFgsSetupTaxAuthorityReadRepository taxAuthorityReadRepository)
    {
        RuleFor(x => x.FgsSetupTaxAuthorityId).GreaterThan(0);
        RuleFor(x => x.FgsSetupTaxAuthorityId).MustAsync(async (assignment, id, cancellationToken) =>
                await taxAuthorityReadRepository.ExistsByIdAsync(id, cancellationToken))
            .WithMessage("The specified tax authority was not found.");
        RuleFor(x => x).Must(dto => !dto.EffectiveToDate.HasValue || dto.EffectiveToDate.Value >= dto.EffectiveFromDate)
            .WithMessage("EffectiveToDate must be greater than or equal to EffectiveFromDate.");
    }
}
