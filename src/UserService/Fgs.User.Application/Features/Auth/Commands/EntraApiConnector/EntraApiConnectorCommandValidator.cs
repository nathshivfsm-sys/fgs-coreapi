using FluentValidation;

namespace Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;

public sealed class EntraApiConnectorCommandValidator : AbstractValidator<EntraApiConnectorCommand>
{
    public EntraApiConnectorCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Email) || !string.IsNullOrWhiteSpace(x.ObjectId))
            .WithMessage("Email or objectId is required.");
    }
}
