using FluentValidation;

namespace UserService.Application.Signup.CreateCompanySignup;

public sealed class CreateCompanySignupCommandValidator : AbstractValidator<CreateCompanySignupCommand>
{
    public CreateCompanySignupCommandValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.AdminEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(320);

        RuleFor(x => x.AdminDisplayName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.AdminDisplayName));
    }
}
