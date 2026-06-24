using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.CreateFgsSetupTaxAuthority;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.PatchFgsSetupTaxAuthority;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.UpdateFgsSetupTaxAuthority;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Validators;

public sealed class CreateFgsSetupTaxAuthorityCommandValidator : AbstractValidator<CreateFgsSetupTaxAuthorityCommand>
{
    public CreateFgsSetupTaxAuthorityCommandValidator(IFgsSetupTaxAuthorityReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A tax authority with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.RegionCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("RegionCode must be uppercase.");



        RuleFor(x => x.Dto.TaxPercent).InclusiveBetween(0m, 100m);
    }
}

public sealed class UpdateFgsSetupTaxAuthorityCommandValidator : AbstractValidator<UpdateFgsSetupTaxAuthorityCommand>
{
    public UpdateFgsSetupTaxAuthorityCommandValidator(IFgsSetupTaxAuthorityReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A tax authority with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.RegionCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("RegionCode must be uppercase.");



        RuleFor(x => x.Dto.TaxPercent).InclusiveBetween(0m, 100m);
    }
}

public sealed class PatchFgsSetupTaxAuthorityCommandValidator : AbstractValidator<PatchFgsSetupTaxAuthorityCommand>
{
    public PatchFgsSetupTaxAuthorityCommandValidator(IFgsSetupTaxAuthorityReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A tax authority with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.RegionCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("RegionCode must be uppercase.").When(x => x.Dto.RegionCode is not null);



        RuleFor(x => x.Dto.TaxPercent).InclusiveBetween(0m, 100m);
    }
}
