using FluentValidation;

namespace Fgs.User.Application.Features.Auth.Commands.StartLogin;

public sealed class StartLoginCommandValidator : AbstractValidator<StartLoginCommand>
{
    public StartLoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.");
    }
}
