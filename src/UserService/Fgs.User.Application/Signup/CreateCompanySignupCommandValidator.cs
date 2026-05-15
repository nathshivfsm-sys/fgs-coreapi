using FluentValidation;

namespace Fgs.User.Application.Signup;

public sealed class CreateCompanySignupCommandValidator : AbstractValidator<CreateCompanySignupCommand>
{
    public CreateCompanySignupCommandValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty()
            .MaximumLength(50)
            .Matches("^[A-Za-z0-9_-]+$");

        RuleFor(x => x.TenantName).NotEmpty().MaximumLength(200);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(300);

        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(200);

        RuleFor(x => x.Website)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Website));

        RuleFor(x => x.TimeZone)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.TimeZone));

        RuleFor(x => x.DefaultCurrency)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.DefaultCurrency));

        When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
        {
            RuleFor(x => x.Password!)
                .MinimumLength(12)
                .MaximumLength(128)
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain a digit.")
                .Matches(@"[\W_]").WithMessage("Password must contain a special character.");
        });
    }
}
