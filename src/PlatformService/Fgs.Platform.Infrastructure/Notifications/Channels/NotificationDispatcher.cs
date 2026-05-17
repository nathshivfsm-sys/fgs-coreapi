using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Application.Notifications.Channels.Models;
using Fgs.Platform.Application.Notifications.History;
using Fgs.Platform.Application.Notifications.Providers;
using Fgs.Platform.Application.Notifications.Templates;
using Fgs.Platform.Domain.Entities;
using Fgs.Platform.Domain.Notifications;
using Microsoft.Extensions.Logging;

namespace Fgs.Platform.Infrastructure.Notifications.Channels;

public sealed class NotificationDispatcher(
    INotificationProviderFactory providerFactory,
    INotificationTemplateRenderer templateRenderer,
    INotificationHistoryRepository historyRepository,
    ILogger<NotificationDispatcher> logger) : INotificationDispatcher
{
    public async Task<NotificationDispatchResult> DispatchAsync(
        NotificationDispatchRequest request,
        CancellationToken cancellationToken = default)
    {
        var history = new FgsNotificationHistory
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            Channel = request.Channel,
            TemplateName = request.TemplateCode,
            Recipient = request.Recipient,
            Status = NotificationDeliveryStatus.Pending,
            CorrelationId = request.CorrelationId,
            CreatedOn = DateTimeOffset.UtcNow
        };

        await historyRepository.AddAsync(history, cancellationToken);

        try
        {
            var result = request.Channel switch
            {
                NotificationChannel.Email => await DispatchEmailAsync(request, cancellationToken),
                NotificationChannel.Sms => await DispatchSmsAsync(request, cancellationToken),
                NotificationChannel.Push => await DispatchPushAsync(request, cancellationToken),
                _ => new NotificationDispatchResult(false, null, "Unsupported channel.")
            };

            await historyRepository.UpdateStatusAsync(
                history.Id,
                result.Success ? NotificationDeliveryStatus.Sent : NotificationDeliveryStatus.Failed,
                result.ProviderMessageId,
                result.Error,
                result.Success ? DateTimeOffset.UtcNow : null,
                cancellationToken);

            if (!result.Success)
            {
                logger.LogWarning(
                    "Notification dispatch failed (TenantId={TenantId}, Channel={Channel}, Template={Template}, CorrelationId={CorrelationId}): {Error}",
                    request.TenantId,
                    request.Channel,
                    request.TemplateCode,
                    request.CorrelationId,
                    result.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            await historyRepository.UpdateStatusAsync(
                history.Id,
                NotificationDeliveryStatus.Failed,
                null,
                ex.Message,
                null,
                cancellationToken);
            throw;
        }
    }

    private async Task<NotificationDispatchResult> DispatchEmailAsync(
        NotificationDispatchRequest request,
        CancellationToken cancellationToken)
    {
        var rendered = await templateRenderer.RenderAsync(
            request.TenantId,
            request.CompanyId,
            request.Channel,
            request.TemplateCode,
            request.TemplateData,
            cancellationToken);
        var provider = providerFactory.ResolveEmailProvider(request.TenantId);
        return await provider.SendAsync(
            new EmailNotificationMessage(
                request.TenantId,
                request.Recipient,
                request.TemplateData.GetValueOrDefault("DisplayName"),
                rendered.Subject,
                rendered.HtmlBody,
                rendered.PlainTextBody,
                request.CorrelationId),
            cancellationToken);
    }

    private async Task<NotificationDispatchResult> DispatchSmsAsync(
        NotificationDispatchRequest request,
        CancellationToken cancellationToken)
    {
        var rendered = await templateRenderer.RenderAsync(
            request.TenantId,
            request.CompanyId,
            request.Channel,
            request.TemplateCode,
            request.TemplateData,
            cancellationToken);
        var provider = providerFactory.ResolveSmsProvider(request.TenantId);
        return await provider.SendAsync(
            new SmsNotificationMessage(
                request.TenantId,
                request.Recipient,
                rendered.PlainTextBody,
                request.CorrelationId),
            cancellationToken);
    }

    private async Task<NotificationDispatchResult> DispatchPushAsync(
        NotificationDispatchRequest request,
        CancellationToken cancellationToken)
    {
        var rendered = await templateRenderer.RenderAsync(
            request.TenantId,
            request.CompanyId,
            request.Channel,
            request.TemplateCode,
            request.TemplateData,
            cancellationToken);
        var provider = providerFactory.ResolvePushProvider(request.TenantId);
        return await provider.SendAsync(
            new PushNotificationMessage(
                request.TenantId,
                request.Recipient,
                rendered.Subject,
                rendered.PlainTextBody,
                request.TemplateData,
                request.CorrelationId),
            cancellationToken);
    }
}
