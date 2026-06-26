using Fgs.Notification.Application.Integrations.Stripe;

namespace Fgs.Notification.Infrastructure.Integrations.Stripe;

public sealed class StripeIntegrationClient : IStripeIntegrationClient
{
    public string IntegrationName => "Stripe";
}
