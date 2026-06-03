namespace Fgs.Notification.Infrastructure.Credentials;

/// <summary>
/// Blocks RabbitMQ consumers until the first credential load attempt has finished so
/// <see cref="RabbitMqCredentialOptionsPostConfigure"/> can apply vault connection settings.
/// </summary>
public sealed class RabbitMqConsumerStartupGate
{
    private readonly TaskCompletionSource _released = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public void Release() => _released.TrySetResult();

    public Task WaitAsync(CancellationToken cancellationToken) =>
        _released.Task.WaitAsync(cancellationToken);
}
