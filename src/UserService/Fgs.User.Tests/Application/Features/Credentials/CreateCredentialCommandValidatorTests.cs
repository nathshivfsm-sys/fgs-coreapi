using System.Text.Json;
using Fgs.User.Application.Features.Credentials.Commands.CreateCredential;
using FluentValidation.TestHelper;

namespace Fgs.User.Tests.Application.Features.Credentials;

public sealed class CreateCredentialCommandValidatorTests
{
    private readonly CreateCredentialCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_secret_payload_missing()
    {
        var command = new CreateCredentialCommand
        {
            TenantId = 1,
            CompanyId = 1,
            ProviderCode = "STRIPE",
            SecretPayload = default
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.SecretPayload.ValueKind);
    }

    [Fact]
    public void Should_pass_with_valid_command()
    {
        var command = new CreateCredentialCommand
        {
            TenantId = 1,
            CompanyId = 1,
            CredentialProviderTypeId = 4,
            ProviderCode = "STRIPE",
            ProviderName = "Stripe Production",
            Environment = "Production",
            SecretPayload = JsonDocument.Parse("""{"secretKey":"sk_test"}""").RootElement
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
