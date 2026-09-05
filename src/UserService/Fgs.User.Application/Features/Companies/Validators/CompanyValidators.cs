using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Companies.Commands.CreateCompany;
using Fgs.User.Application.Features.Companies.Commands.PatchCompany;
using Fgs.User.Application.Features.Companies.Commands.UpdateCompany;
using Fgs.User.Application.Features.Signup;
using FluentValidation;

namespace Fgs.User.Application.Features.Companies.Validators;

public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.LegalName).MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Dto.LegalName));
        RuleFor(x => x.Dto.Email).EmailAddress().MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Dto.Email));
        RuleFor(x => x.Dto.PhoneNumber)
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.PhoneNumber))
            .WithMessage(SignupErrorMessages.InvalidPhoneFormat);
        RuleFor(x => x.Dto.Website).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Dto.Website));
        RuleFor(x => x.Dto.TaxId).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Dto.TaxId));
        RuleFor(x => x.Dto.CompanySize).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Dto.CompanySize));
        RuleFor(x => x.Dto.TimeZone).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Dto.TimeZone));
        RuleFor(x => x.Dto.PhysicalAddress).SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.Dto.PhysicalAddress is not null);
        RuleFor(x => x.Dto.BillingAddress).SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.Dto.BillingAddress is not null);
    }
}

public sealed class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.CompanyId).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.LegalName).MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Dto.LegalName));
        RuleFor(x => x.Dto.Email).EmailAddress().MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Dto.Email));
        RuleFor(x => x.Dto.PhoneNumber)
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.PhoneNumber))
            .WithMessage(SignupErrorMessages.InvalidPhoneFormat);
        RuleFor(x => x.Dto.Website).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Dto.Website));
        RuleFor(x => x.Dto.TaxId).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Dto.TaxId));
        RuleFor(x => x.Dto.CompanySize).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Dto.CompanySize));
        RuleFor(x => x.Dto.TimeZone).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Dto.TimeZone));
        RuleFor(x => x.Dto.PhysicalAddress).SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.Dto.PhysicalAddress is not null);
        RuleFor(x => x.Dto.BillingAddress).SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.Dto.BillingAddress is not null);
    }
}

public sealed class PatchCompanyCommandValidator : AbstractValidator<PatchCompanyCommand>
{
    public PatchCompanyCommandValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.CompanyId).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.LegalName).MaximumLength(300).When(x => x.Dto.LegalName is not null);
        RuleFor(x => x.Dto.Email).EmailAddress().MaximumLength(300)
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.Email));
        RuleFor(x => x.Dto.PhoneNumber)
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.PhoneNumber))
            .WithMessage(SignupErrorMessages.InvalidPhoneFormat);
        RuleFor(x => x.Dto.Website).MaximumLength(500).When(x => x.Dto.Website is not null);
        RuleFor(x => x.Dto.TaxId).MaximumLength(100).When(x => x.Dto.TaxId is not null);
        RuleFor(x => x.Dto.CompanySize).MaximumLength(20).When(x => x.Dto.CompanySize is not null);
        RuleFor(x => x.Dto.TimeZone).MaximumLength(100).When(x => x.Dto.TimeZone is not null);
        RuleFor(x => x.Dto.PhysicalAddress).SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.Dto.PhysicalAddress is not null);
        RuleFor(x => x.Dto.BillingAddress).SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.Dto.BillingAddress is not null);
    }
}
