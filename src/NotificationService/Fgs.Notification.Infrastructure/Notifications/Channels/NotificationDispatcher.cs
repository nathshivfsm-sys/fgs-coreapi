using System.Text.Json;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.History;
using Fgs.Notification.Application.Notifications.Providers;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Enums;
using Fgs.Notification.Domain.Notifications;
using Microsoft.Extensions.Logging;

namespace Fgs.Notification.Infrastructure.Notifications.Channels;

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
        return request.Channel switch
        {
            NotificationChannel.Email => await DispatchEmailWithHistoryAsync(request, cancellationToken),
            NotificationChannel.Sms => await DispatchSmsWithHistoryAsync(request, cancellationToken),
            NotificationChannel.Push => await DispatchPushAsync(request, cancellationToken),
            _ => new NotificationDispatchResult(false, null, "Unsupported channel.")
        };
    }

    private async Task<NotificationDispatchResult> DispatchEmailWithHistoryAsync(
        NotificationDispatchRequest request,
        CancellationToken cancellationToken)
    {
        var companyId = request.CompanyId ?? 0;
        var rendered = await templateRenderer.RenderAsync(
            request.TenantId,
            request.CompanyId,
            request.Channel,
            request.TemplateCode,
            request.TemplateData,
            cancellationToken);

        var history = new FgsEmailHistory
        {
            TenantId = request.TenantId,
            CompanyId = companyId,
            RecordType = request.TemplateData.GetValueOrDefault("RecordType") ?? "SYSTEM",
            RecordId = ParseRecordId(request.TemplateData),
            Status = NotificationStatus.Queued,
            SourceApplication = NotificationSourceApplication.Api,
            Subject = rendered.Subject ?? request.TemplateCode,
            FromEmailAddress = request.TemplateData.GetValueOrDefault("FromEmailAddress") ?? "noreply@fgs.local",
            FromDisplayName = request.TemplateData.GetValueOrDefault("DisplayName"),
            ToEmailAddresses = JsonSerializer.Serialize(new[] { request.Recipient }),
            Body = rendered.HtmlBody ?? rendered.PlainTextBody ?? string.Empty,
            CreatedOn = DateTimeOffset.UtcNow
        };

        var historyId = await historyRepository.AddEmailAsync(history, cancellationToken);

        try
        {
            var provider = providerFactory.ResolveEmailProvider(request.TenantId);
            var result = await provider.SendAsync(
                new EmailNotificationMessage(
                    request.TenantId,
                    request.Recipient,
                    request.TemplateData.GetValueOrDefault("DisplayName"),
                    rendered.Subject ?? request.TemplateCode,
                    rendered.HtmlBody ?? rendered.PlainTextBody ?? string.Empty,
                    rendered.PlainTextBody,
                    request.CorrelationId),
                cancellationToken);

            await historyRepository.UpdateEmailStatusAsync(
                historyId,
                result.Success ? NotificationStatus.Sent : NotificationStatus.Failed,
                result.ProviderMessageId,
                provider.ProviderName,
                result.Error,
                result.Success ? DateTimeOffset.UtcNow : null,
                result.Success ? null : DateTimeOffset.UtcNow,
                cancellationToken);

            if (!result.Success)
            {
                LogDispatchFailure(request, result.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            await historyRepository.UpdateEmailStatusAsync(
                historyId,
                NotificationStatus.Failed,
                null,
                null,
                ex.Message,
                null,
                DateTimeOffset.UtcNow,
                cancellationToken);
            throw;
        }
    }

    private async Task<NotificationDispatchResult> DispatchSmsWithHistoryAsync(
        NotificationDispatchRequest request,
        CancellationToken cancellationToken)
    {
        var companyId = request.CompanyId ?? 0;
        var rendered = await templateRenderer.RenderAsync(
            request.TenantId,
            request.CompanyId,
            request.Channel,
            request.TemplateCode,
            request.TemplateData,
            cancellationToken);

        var history = new FgsSmsHistory
        {
            TenantId = request.TenantId,
            CompanyId = companyId,
            RecordType = request.TemplateData.GetValueOrDefault("RecordType") ?? "SYSTEM",
            RecordId = ParseRecordId(request.TemplateData),
            Status = NotificationStatus.Queued,
            SourceApplication = NotificationSourceApplication.Api,
            FromPhoneNumber = request.TemplateData.GetValueOrDefault("FromPhoneNumber") ?? "0000000000",
            ToPhoneNumber = request.Recipient,
            Message = rendered.PlainTextBody ?? string.Empty,
            CreatedOn = DateTimeOffset.UtcNow
        };

        var historyId = await historyRepository.AddSmsAsync(history, cancellationToken);

        try
        {
            var provider = providerFactory.ResolveSmsProvider(request.TenantId);
            var result = await provider.SendAsync(
                new SmsNotificationMessage(
                    request.TenantId,
                    request.Recipient,
                    rendered.PlainTextBody ?? string.Empty,
                    request.CorrelationId),
                cancellationToken);

            await historyRepository.UpdateSmsStatusAsync(
                historyId,
                result.Success ? NotificationStatus.Sent : NotificationStatus.Failed,
                result.ProviderMessageId,
                provider.ProviderName,
                result.Error,
                result.Success ? DateTimeOffset.UtcNow : null,
                result.Success ? null : DateTimeOffset.UtcNow,
                cancellationToken);

            if (!result.Success)
            {
                LogDispatchFailure(request, result.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            await historyRepository.UpdateSmsStatusAsync(
                historyId,
                NotificationStatus.Failed,
                null,
                null,
                ex.Message,
                null,
                DateTimeOffset.UtcNow,
                cancellationToken);
            throw;
        }
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
        var result = await provider.SendAsync(
            new PushNotificationMessage(
                request.TenantId,
                request.Recipient,
                rendered.Subject,
                rendered.PlainTextBody,
                request.TemplateData,
                request.CorrelationId),
            cancellationToken);

        if (!result.Success)
        {
            LogDispatchFailure(request, result.Error);
        }

        return result;
    }

    private void LogDispatchFailure(NotificationDispatchRequest request, string? error) =>
        logger.LogWarning(
            "Notification dispatch failed (TenantId={TenantId}, Channel={Channel}, Template={Template}, CorrelationId={CorrelationId}): {Error}",
            request.TenantId,
            request.Channel,
            request.TemplateCode,
            request.CorrelationId,
            error);

    private static long ParseRecordId(IReadOnlyDictionary<string, string> templateData) =>
        templateData.TryGetValue("RecordId", out var raw) && long.TryParse(raw, out var id) ? id : 0;
}
