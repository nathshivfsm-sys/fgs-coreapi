using FluentValidation;

namespace Fgs.User.Application.Features.Credentials.Commands.RotateCredential;

public sealed class RotateCredentialCommandValidator : AbstractValidator<RotateCredentialCommand>
{
    public RotateCredentialCommandValidator()
    {
        RuleFor(x => x.SecretId).NotEmpty();
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.CompanyId).GreaterThan(0);
    }
}
