namespace Fgs.User.Application.Features.Credentials.Payloads;

public sealed class StripeSecretPayload
{
    public string PublishableKey { get; init; } = null!;

    public string SecretKey { get; init; } = null!;

    public string WebhookSecret { get; init; } = null!;
}
