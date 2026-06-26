namespace Fgs.Notification.Application.BackgroundJobs;

public interface IBackgroundJobQueue
{
    ValueTask EnqueueAsync(BackgroundJob job, CancellationToken cancellationToken = default);

    ValueTask<BackgroundJob?> DequeueAsync(CancellationToken cancellationToken = default);
}

public sealed record BackgroundJob(
    string JobType,
    string Payload,
    string? CorrelationId,
    int Attempt = 0);
