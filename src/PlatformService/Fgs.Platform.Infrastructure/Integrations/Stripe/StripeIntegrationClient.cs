using Fgs.Platform.Application.Integrations.Stripe;

namespace Fgs.Platform.Infrastructure.Integrations.Stripe;

public sealed class StripeIntegrationClient : IStripeIntegrationClient
{
    public string IntegrationName => "Stripe";
}
