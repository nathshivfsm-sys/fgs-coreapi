using Fgs.Notification.Application.Integrations.Twilio;

namespace Fgs.Notification.Infrastructure.Integrations.Twilio;

public sealed class TwilioIntegrationClient : ITwilioIntegrationClient
{
    public string IntegrationName => "Twilio";
}
