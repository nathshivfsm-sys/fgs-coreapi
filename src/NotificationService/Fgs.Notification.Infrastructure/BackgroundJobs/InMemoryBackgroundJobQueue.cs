using System.Threading.Channels;
using Fgs.Notification.Application.BackgroundJobs;

namespace Fgs.Notification.Infrastructure.BackgroundJobs;

public sealed class InMemoryBackgroundJobQueue : IBackgroundJobQueue
{
    private readonly Channel<BackgroundJob> _channel = Channel.CreateUnbounded<BackgroundJob>();

    public ValueTask EnqueueAsync(BackgroundJob job, CancellationToken cancellationToken = default) =>
        _channel.Writer.WriteAsync(job, cancellationToken);

    public async ValueTask<BackgroundJob?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        if (await _channel.Reader.WaitToReadAsync(cancellationToken)
            && _channel.Reader.TryRead(out var job))
        {
            return job;
        }

        return null;
    }
}
