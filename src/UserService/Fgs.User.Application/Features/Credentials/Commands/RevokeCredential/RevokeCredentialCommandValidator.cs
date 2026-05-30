using FluentValidation;

namespace Fgs.User.Application.Features.Credentials.Commands.RevokeCredential;

public sealed class RevokeCredentialCommandValidator : AbstractValidator<RevokeCredentialCommand>
{
    public RevokeCredentialCommandValidator()
    {
        RuleFor(x => x.SecretId).NotEmpty();
        RuleFor(x => x.TenantId).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CompanyId).GreaterThanOrEqualTo(0);
    }
}
