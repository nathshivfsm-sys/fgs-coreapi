using FluentValidation;

namespace Fgs.User.Application.Features.Auth.Commands.EntraCallback;

public sealed class EntraCallbackCommandValidator : AbstractValidator<EntraCallbackCommand>
{
    public EntraCallbackCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
    }
}
