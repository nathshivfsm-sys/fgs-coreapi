using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Tenants.Dtos;
using FluentValidation;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenantCompanyDetails;

public sealed class UpdateTenantCompanyDetailsCommandValidator
    : AbstractValidator<UpdateTenantCompanyDetailsCommand>
{
    public UpdateTenantCompanyDetailsCommandValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.CompanyId).GreaterThan(0);
        RuleFor(x => x.Request.Tenant).NotNull().SetValidator(new UpdateTenantSectionRequestValidator());
        RuleFor(x => x.Request.Company).NotNull().SetValidator(new UpdateCompanySectionRequestValidator());
    }
}

public sealed class UpdateTenantSectionRequestValidator : AbstractValidator<UpdateTenantSectionRequest>
{
    public UpdateTenantSectionRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LegalName).MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.LegalName));
        RuleFor(x => x.Email).EmailAddress().MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage(SignupErrorMessages.InvalidPhoneFormat);
        RuleFor(x => x.Website).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Website));
        RuleFor(x => x.DefaultCurrency).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.DefaultCurrency));
    }
}

public sealed class UpdateCompanySectionRequestValidator : AbstractValidator<UpdateCompanySectionRequest>
{
    public UpdateCompanySectionRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LegalName).MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.LegalName));
        RuleFor(x => x.Email).EmailAddress().MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage(SignupErrorMessages.InvalidPhoneFormat);
        RuleFor(x => x.Website).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Website));
        RuleFor(x => x.TaxId).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.TaxId));
        RuleFor(x => x.CompanySize).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.CompanySize));
        RuleFor(x => x.TimeZone).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.TimeZone));
        RuleFor(x => x.PhysicalAddress).SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.PhysicalAddress is not null);
        RuleFor(x => x.BillingAddress).SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.BillingAddress is not null);
    }
}

public sealed class LocationWriteDtoValidator : AbstractValidator<LocationWriteDto>
{
    public LocationWriteDtoValidator()
    {
        RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AddressLine2).MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.AddressLine2));
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.County).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.County));
        RuleFor(x => x.Country).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Country));
        RuleFor(x => x.PlaceId).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.PlaceId));
    }
}
