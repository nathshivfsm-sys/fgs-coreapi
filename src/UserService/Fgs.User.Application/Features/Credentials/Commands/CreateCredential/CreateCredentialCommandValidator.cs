using FluentValidation;
using Fgs.User.Application.Features.Credentials;

namespace Fgs.User.Application.Features.Credentials.Commands.CreateCredential;

public sealed class CreateCredentialCommandValidator : AbstractValidator<CreateCredentialCommand>
{
    public CreateCredentialCommandValidator()
    {
        RuleFor(x => x.TenantId).GreaterThanOrEqualTo(0).WithMessage(CredentialErrorMessages.TenantIdRequired);
        RuleFor(x => x.CompanyId).GreaterThanOrEqualTo(0).WithMessage(CredentialErrorMessages.CompanyIdRequired);
        RuleFor(x => x.ProviderCode).NotEmpty().WithMessage(CredentialErrorMessages.ProviderCodeRequired);
        RuleFor(x => x.SecretPayload.ValueKind).NotEqual(System.Text.Json.JsonValueKind.Undefined)
            .WithMessage(CredentialErrorMessages.SecretPayloadRequired);
    }
}
