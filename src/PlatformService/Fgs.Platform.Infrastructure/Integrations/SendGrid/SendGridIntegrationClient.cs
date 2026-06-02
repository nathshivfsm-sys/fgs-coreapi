using Fgs.Platform.Application.Integrations.SendGrid;
using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Application.Notifications.Channels.Models;
using Fgs.Platform.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Fgs.Platform.Infrastructure.Integrations.SendGrid;

public sealed class SendGridIntegrationClient(
    IOptionsMonitor<SendGridOptions> options,
    ILogger<SendGridIntegrationClient> logger) : ISendGridIntegrationClient
{
    private SendGridOptions Options => options.CurrentValue;

    public string IntegrationName => "SendGrid";

    public async Task<NotificationDispatchResult> SendEmailAsync(
        EmailNotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(Options.ApiKey) || Options.ApiKey == "REPLACE_WITH_SENDGRID_API_KEY")
        {
            logger.LogWarning(
                "SendGrid API key is not configured; skipping send (CorrelationId={CorrelationId}).",
                message.CorrelationId);
            return new NotificationDispatchResult(false, null, "SendGrid API key is not configured.");
        }

        var client = new SendGridClient(Options.ApiKey);
        var from = new EmailAddress(Options.FromAddress, Options.FromName);
        var to = new EmailAddress(message.ToAddress, message.ToName);
        var mail = MailHelper.CreateSingleEmail(
            from,
            to,
            message.Subject,
            message.PlainTextBody ?? message.Subject,
            message.HtmlBody);

        var response = await client.SendEmailAsync(mail, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var messageId = response.Headers.TryGetValues("X-Message-Id", out var ids)
                ? ids.FirstOrDefault()
                : Guid.NewGuid().ToString("N");

            logger.LogInformation(
                "SendGrid email sent (Status={Status}, CorrelationId={CorrelationId}).",
                response.StatusCode,
                message.CorrelationId);

            return new NotificationDispatchResult(true, messageId, null);
        }

        var body = await response.Body.ReadAsStringAsync(cancellationToken);
        logger.LogError(
            "SendGrid send failed (Status={Status}, CorrelationId={CorrelationId}): {Body}",
            response.StatusCode,
            message.CorrelationId,
            body);

        return new NotificationDispatchResult(false, null, $"SendGrid error: {response.StatusCode}");
    }
}
