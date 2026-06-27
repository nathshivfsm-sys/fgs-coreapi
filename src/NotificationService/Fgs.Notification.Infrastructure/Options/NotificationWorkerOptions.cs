using Fgs.Notification.Infrastructure.Options;
namespace Fgs.Notification.Infrastructure.Options;

public sealed class NotificationWorkerOptions
{
    public const string SectionName = "NotificationWorker";

    public int MaxRetryAttempts { get; set; } = 5;

    public int RetryDelaySeconds { get; set; } = 10;

    public ushort PrefetchCount { get; set; } = 10;
}
