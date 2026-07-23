using Fgs.Contracts.Signup;
using FluentValidation;

namespace Fgs.Bff.Application.Features.Signup.Commands.CreateCompanySignup;

public sealed class CreateCompanySignupCommandValidator : AbstractValidator<CreateCompanySignupCommand>
{
    public CreateCompanySignupCommandValidator()
    {
        RuleFor(x => x.Contact).NotNull().SetValidator(new SignupContactDtoValidator());
        RuleFor(x => x.Company).NotNull().SetValidator(new SignupCompanyDtoValidator());

        RuleFor(x => x.BusinessTypeIds)
            .NotEmpty()
            .WithMessage(SignupErrorMessages.BusinessTypeIdsRequired);

        RuleForEach(x => x.BusinessTypeIds).GreaterThan(0);

        RuleFor(x => x.TimeZone)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.TimeZone));

        RuleFor(x => x.DefaultCurrency)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.DefaultCurrency));
    }
}

public sealed class SignupContactDtoValidator : AbstractValidator<SignupContactDto>
{
    public SignupContactDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .WithMessage(SignupErrorMessages.InvalidPhoneFormat);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(300);
    }
}

public sealed class SignupCompanyDtoValidator : AbstractValidator<SignupCompanyDto>
{
    public SignupCompanyDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);

        RuleFor(x => x.Website)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Website));

        RuleFor(x => x.Address).NotNull().SetValidator(new SignupAddressDtoValidator());

        RuleFor(x => x.CompanySize).NotEmpty().MaximumLength(20);
    }
}

public sealed class SignupAddressDtoValidator : AbstractValidator<SignupAddressDto>
{
    public SignupAddressDtoValidator()
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

internal static class SignupErrorMessages
{
    public const string BusinessTypeIdsRequired = "At least one industry (business type) must be selected.";
    public const string InvalidPhoneFormat = "Phone number format is invalid.";
}
