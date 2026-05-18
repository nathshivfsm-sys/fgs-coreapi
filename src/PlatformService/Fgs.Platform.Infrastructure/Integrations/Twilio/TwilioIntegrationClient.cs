using Fgs.Platform.Application.Integrations.Twilio;

namespace Fgs.Platform.Infrastructure.Integrations.Twilio;

public sealed class TwilioIntegrationClient : ITwilioIntegrationClient
{
    public string IntegrationName => "Twilio";
}
