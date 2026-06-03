using Microsoft.Extensions.Primitives;

namespace Fgs.Notification.Infrastructure.Credentials;

public sealed class CredentialOptionsChangeNotifier
{
    private CancellationTokenSource _changeTokenSource = new();

    public IChangeToken GetChangeToken() => new CancellationChangeToken(_changeTokenSource.Token);

    public void NotifyChange()
    {
        var previous = Interlocked.Exchange(ref _changeTokenSource, new CancellationTokenSource());
        previous.Cancel();
        previous.Dispose();
    }
}
