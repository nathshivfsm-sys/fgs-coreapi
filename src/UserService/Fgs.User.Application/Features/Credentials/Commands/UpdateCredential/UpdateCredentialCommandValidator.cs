using FluentValidation;

namespace Fgs.User.Application.Features.Credentials.Commands.UpdateCredential;

public sealed class UpdateCredentialCommandValidator : AbstractValidator<UpdateCredentialCommand>
{
    public UpdateCredentialCommandValidator()
    {
        RuleFor(x => x.SecretId).NotEmpty();
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.CompanyId).GreaterThan(0);
    }
}
