using Fgs.User.Application.Abstractions.Credentials;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.User.Infrastructure.Credentials;

public sealed class CredentialConfigurationProvider : ICredentialConfigurationProvider
{
    private readonly CredentialConfigurationHolder _holder;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly CredentialOptionsChangeNotifier _changeNotifier;

    public CredentialConfigurationProvider(
        CredentialConfigurationHolder holder,
        IServiceScopeFactory scopeFactory,
        CredentialOptionsChangeNotifier changeNotifier)
    {
        _holder = holder;
        _scopeFactory = scopeFactory;
        _changeNotifier = changeNotifier;
    }

    public IReadOnlyDictionary<string, string> Values => _holder.Values;

    public string? GetValue(string key) => _holder.GetValue(key);

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();
        var loader = scope.ServiceProvider.GetRequiredService<CredentialConfigurationLoader>();
        await loader.ReloadAsync(cancellationToken);
        _changeNotifier.NotifyChange();

        var changePublisher = scope.ServiceProvider.GetService<ICredentialConfigurationChangePublisher>();
        if (changePublisher is not null)
        {
            await changePublisher.PublishAsync(cancellationToken);
        }
    }
}
