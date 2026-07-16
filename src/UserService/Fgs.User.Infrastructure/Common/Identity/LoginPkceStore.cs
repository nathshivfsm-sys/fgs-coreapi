using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.User.Application.Abstractions.Identity;

namespace Fgs.User.Infrastructure.Common.Identity;

public sealed class LoginPkceStore(ICacheService cache) : ILoginPkceStore
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(10);

    public Task SaveAsync(string state, LoginPkceState pkceState, CancellationToken cancellationToken = default) =>
        cache.SetAsync(CacheKeys.LoginPkceByState(state), pkceState, Ttl, cancellationToken);

    public async Task<LoginPkceState?> TakeAsync(string state, CancellationToken cancellationToken = default)
    {
        var key = CacheKeys.LoginPkceByState(state);
        var value = await cache.GetAsync<LoginPkceState>(key, cancellationToken);
        if (value is not null)
        {
            await cache.RemoveAsync(key, cancellationToken);
        }

        return value;
    }
}
