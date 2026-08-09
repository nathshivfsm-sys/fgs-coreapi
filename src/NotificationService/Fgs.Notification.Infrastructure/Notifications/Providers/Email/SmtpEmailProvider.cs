using System.Net;
using System.Net.Mail;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Notification.Infrastructure.Notifications.Providers.Email;

public sealed class SmtpEmailProvider(
    IOptionsMonitor<SmtpOptions> options,
    ILogger<SmtpEmailProvider> logger) : IEmailProvider
{
    public string ProviderName => "Smtp";

    public async Task<NotificationDispatchResult> SendAsync(
        EmailNotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        var smtp = options.CurrentValue;
        if (string.IsNullOrWhiteSpace(smtp.Host))
        {
            logger.LogWarning(
                "SMTP host is not configured; email not sent (TenantId={TenantId}, To={To}, CorrelationId={CorrelationId}).",
                message.TenantId,
                message.ToAddress,
                message.CorrelationId);

            return new NotificationDispatchResult(false, null, "SMTP host is not configured.");
        }

        using var mail = new MailMessage
        {
            From = new MailAddress(smtp.FromAddress, smtp.FromName),
            Subject = message.Subject,
            Body = string.IsNullOrWhiteSpace(message.HtmlBody)
                ? message.PlainTextBody ?? message.Subject
                : message.HtmlBody,
            IsBodyHtml = !string.IsNullOrWhiteSpace(message.HtmlBody)
        };
        mail.To.Add(new MailAddress(message.ToAddress, message.ToName ?? string.Empty));

        using var client = new SmtpClient(smtp.Host, smtp.Port)
        {
            EnableSsl = smtp.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        if (!string.IsNullOrWhiteSpace(smtp.UserName))
        {
            client.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
        }

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await client.SendMailAsync(mail, cancellationToken);
            var messageId = Guid.NewGuid().ToString("N");
            logger.LogInformation(
                "SMTP email sent (CorrelationId={CorrelationId}, MessageId={MessageId}).",
                message.CorrelationId,
                messageId);
            return new NotificationDispatchResult(true, messageId, null);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(
                ex,
                "SMTP send failed (CorrelationId={CorrelationId}).",
                message.CorrelationId);
            return new NotificationDispatchResult(false, null, $"SMTP error: {ex.Message}");
        }
    }
}
