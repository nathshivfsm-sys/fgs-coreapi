using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Notification.Infrastructure.Notifications.Providers.Sms;

public sealed class TwilioSmsProvider(
    IHttpClientFactory httpClientFactory,
    IOptionsMonitor<TwilioOptions> options,
    ILogger<TwilioSmsProvider> logger) : ISmsProvider
{
    public string ProviderName => "Twilio";

    public async Task<NotificationDispatchResult> SendAsync(
        SmsNotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        var twilio = options.CurrentValue;
        if (string.IsNullOrWhiteSpace(twilio.AccountSid)
            || string.IsNullOrWhiteSpace(twilio.AuthToken)
            || string.IsNullOrWhiteSpace(twilio.FromNumber)
            || twilio.AccountSid.StartsWith("REPLACE_", StringComparison.Ordinal))
        {
            logger.LogWarning(
                "Twilio is not configured; SMS not sent (TenantId={TenantId}, CorrelationId={CorrelationId}).",
                message.TenantId,
                message.CorrelationId);

            return new NotificationDispatchResult(false, null, "Twilio credentials are not configured.");
        }

        var client = httpClientFactory.CreateClient(nameof(TwilioSmsProvider));
        var url = $"https://api.twilio.com/2010-04-01/Accounts/{twilio.AccountSid}/Messages.json";

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        var authBytes = Encoding.ASCII.GetBytes($"{twilio.AccountSid}:{twilio.AuthToken}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["To"] = message.PhoneNumber,
            ["From"] = twilio.FromNumber,
            ["Body"] = message.Body
        });

        try
        {
            using var response = await client.SendAsync(request, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError(
                    "Twilio send failed (Status={Status}, CorrelationId={CorrelationId}): {Body}",
                    response.StatusCode,
                    message.CorrelationId,
                    body);
                return new NotificationDispatchResult(false, null, $"Twilio error: {response.StatusCode}");
            }

            string? sid = null;
            try
            {
                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("sid", out var sidProp))
                {
                    sid = sidProp.GetString();
                }
            }
            catch (JsonException)
            {
                sid = Guid.NewGuid().ToString("N");
            }

            logger.LogInformation(
                "Twilio SMS sent (Sid={Sid}, CorrelationId={CorrelationId}).",
                sid,
                message.CorrelationId);
            return new NotificationDispatchResult(true, sid, null);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Twilio send failed (CorrelationId={CorrelationId}).", message.CorrelationId);
            return new NotificationDispatchResult(false, null, $"Twilio error: {ex.Message}");
        }
    }
}
