using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Fgs.User.Infrastructure.Credentials;

public sealed class CredentialOptionsChangeTokenSource<TOptions>(CredentialOptionsChangeNotifier notifier)
    : IOptionsChangeTokenSource<TOptions>
    where TOptions : class
{
    public string Name => Options.DefaultName;

    public IChangeToken GetChangeToken() => notifier.GetChangeToken();
}
