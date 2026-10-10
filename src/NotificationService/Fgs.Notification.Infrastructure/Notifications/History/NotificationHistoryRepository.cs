using Fgs.Notification.Application.Notifications.History;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Enums;
using Fgs.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Notification.Infrastructure.Notifications.History;

public sealed class NotificationHistoryRepository(FgsNotificationDbContext context) : INotificationHistoryRepository
{
    public async Task<long> AddEmailAsync(FgsEmailHistory entry, CancellationToken cancellationToken = default)
    {
        await context.FgsEmailHistories.AddAsync(entry, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Id;
    }

    public Task UpdateEmailStatusAsync(
        long id,
        NotificationStatus status,
        string? providerMessageId,
        string? providerName,
        string? failureReason,
        DateTimeOffset? sentOn,
        DateTimeOffset? failedOn,
        CancellationToken cancellationToken = default) =>
        UpdateStatusAsync(
            context.FgsEmailHistories,
            id,
            status,
            providerMessageId,
            providerName,
            failureReason,
            sentOn,
            failedOn,
            cancellationToken);

    public async Task<long> AddSmsAsync(FgsSmsHistory entry, CancellationToken cancellationToken = default)
    {
        await context.FgsSmsHistories.AddAsync(entry, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Id;
    }

    public Task UpdateSmsStatusAsync(
        long id,
        NotificationStatus status,
        string? providerMessageId,
        string? providerName,
        string? failureReason,
        DateTimeOffset? sentOn,
        DateTimeOffset? failedOn,
        CancellationToken cancellationToken = default) =>
        UpdateStatusAsync(
            context.FgsSmsHistories,
            id,
            status,
            providerMessageId,
            providerName,
            failureReason,
            sentOn,
            failedOn,
            cancellationToken);

    private async Task UpdateStatusAsync<THistory>(
        DbSet<THistory> histories,
        long id,
        NotificationStatus status,
        string? providerMessageId,
        string? providerName,
        string? failureReason,
        DateTimeOffset? sentOn,
        DateTimeOffset? failedOn,
        CancellationToken cancellationToken)
        where THistory : class
    {
        var entry = await histories
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(history => EF.Property<long>(history, nameof(FgsEmailHistory.Id)) == id, cancellationToken);

        if (entry is null)
        {
            return;
        }

        var tracked = context.Entry(entry);
        tracked.Property(nameof(FgsEmailHistory.Status)).CurrentValue = status;
        tracked.Property(nameof(FgsEmailHistory.ProviderMessageId)).CurrentValue = providerMessageId;
        var existingProviderName = tracked.Property(nameof(FgsEmailHistory.ProviderName)).CurrentValue as string;
        tracked.Property(nameof(FgsEmailHistory.ProviderName)).CurrentValue = providerName ?? existingProviderName;
        tracked.Property(nameof(FgsEmailHistory.FailureReason)).CurrentValue = failureReason;
        tracked.Property(nameof(FgsEmailHistory.SentOn)).CurrentValue = sentOn;
        tracked.Property(nameof(FgsEmailHistory.FailedOn)).CurrentValue = failedOn;
        await context.SaveChangesAsync(cancellationToken);
    }
}
