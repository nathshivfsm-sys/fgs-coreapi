using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Fgs.Credentials;

public sealed class CredentialOptionsChangeTokenSource<TOptions>(CredentialOptionsChangeNotifier notifier)
    : IOptionsChangeTokenSource<TOptions>
    where TOptions : class
{
    public string Name => Microsoft.Extensions.Options.Options.DefaultName;

    public IChangeToken GetChangeToken() => notifier.GetChangeToken();
}
