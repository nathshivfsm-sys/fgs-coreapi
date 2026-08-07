using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Infrastructure.Options;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Notification.Infrastructure.Notifications.Providers.Push;

public sealed class FirebasePushProvider(
    IOptionsMonitor<FirebaseOptions> options,
    ILogger<FirebasePushProvider> logger) : IPushProvider
{
    private readonly object _gate = new();
    private FirebaseApp? _app;

    public string ProviderName => "Firebase";

    public async Task<NotificationDispatchResult> SendAsync(
        PushNotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        if (!TryEnsureApp(out var error))
        {
            logger.LogWarning(
                "Firebase is not configured; push not sent (TenantId={TenantId}, CorrelationId={CorrelationId}).",
                message.TenantId,
                message.CorrelationId);
            return new NotificationDispatchResult(false, null, error);
        }

        var firebaseMessage = new Message
        {
            Token = message.DeviceToken,
            Notification = new FirebaseAdmin.Messaging.Notification
            {
                Title = message.Title,
                Body = message.Body
            },
            Data = message.Data is null
                ? null
                : message.Data.ToDictionary(static pair => pair.Key, static pair => pair.Value)
        };

        try
        {
            var messageId = await FirebaseMessaging.DefaultInstance.SendAsync(firebaseMessage, cancellationToken);
            logger.LogInformation(
                "Firebase push sent (MessageId={MessageId}, CorrelationId={CorrelationId}).",
                messageId,
                message.CorrelationId);
            return new NotificationDispatchResult(true, messageId, null);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Firebase push failed (CorrelationId={CorrelationId}).", message.CorrelationId);
            return new NotificationDispatchResult(false, null, $"Firebase error: {ex.Message}");
        }
    }

    private bool TryEnsureApp(out string error)
    {
        var firebase = options.CurrentValue;
        var hasPath = !string.IsNullOrWhiteSpace(firebase.CredentialPath)
            && File.Exists(firebase.CredentialPath);
        var hasJson = !string.IsNullOrWhiteSpace(firebase.CredentialJson)
            && !firebase.CredentialJson.Contains("REPLACE_", StringComparison.Ordinal);

        if (!hasPath && !hasJson)
        {
            error = "Firebase credentials are not configured.";
            return false;
        }

        if (_app is not null || FirebaseApp.DefaultInstance is not null)
        {
            error = string.Empty;
            return true;
        }

        lock (_gate)
        {
            if (_app is not null || FirebaseApp.DefaultInstance is not null)
            {
                error = string.Empty;
                return true;
            }

            try
            {
                GoogleCredential credential = hasPath
                    ? GoogleCredential.FromFile(firebase.CredentialPath)
                    : GoogleCredential.FromJson(firebase.CredentialJson);

                _app = FirebaseApp.Create(new AppOptions { Credential = credential });
                error = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                error = $"Firebase credential load failed: {ex.Message}";
                logger.LogError(ex, "Failed to initialize FirebaseApp.");
                return false;
            }
        }
    }
}
