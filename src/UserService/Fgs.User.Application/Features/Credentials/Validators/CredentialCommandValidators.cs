using Fgs.User.Application.Features.Credentials.Commands.CreateCredential;
using FluentValidation;

namespace Fgs.User.Application.Features.Credentials.Validators;

public sealed class CreateCredentialCommandValidator : AbstractValidator<CreateCredentialCommand>
{
    public CreateCredentialCommandValidator()
    {
        RuleFor(x => x.ProviderCode).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CredentialName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Payload).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public sealed class UpdateCredentialCommandValidator : AbstractValidator<Commands.UpdateCredential.UpdateCredentialCommand>
{
    public UpdateCredentialCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CredentialName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
