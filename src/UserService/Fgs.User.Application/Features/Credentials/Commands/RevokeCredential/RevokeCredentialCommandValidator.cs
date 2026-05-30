using FluentValidation;

namespace Fgs.User.Application.Features.Credentials.Commands.RevokeCredential;

public sealed class RevokeCredentialCommandValidator : AbstractValidator<RevokeCredentialCommand>
{
    public RevokeCredentialCommandValidator()
    {
        RuleFor(x => x.SecretId).NotEmpty();
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.CompanyId).GreaterThan(0);
    }
}
