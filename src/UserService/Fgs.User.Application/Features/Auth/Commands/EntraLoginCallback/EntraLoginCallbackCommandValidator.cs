using FluentValidation;

namespace Fgs.User.Application.Features.Auth.Commands.EntraLoginCallback;

public sealed class EntraLoginCallbackCommandValidator : AbstractValidator<EntraLoginCallbackCommand>
{
    public EntraLoginCallbackCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
    }
}
