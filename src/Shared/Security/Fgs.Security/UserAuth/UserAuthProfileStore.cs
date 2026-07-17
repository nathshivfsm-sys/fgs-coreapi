using Fgs.Contracts.Auth;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Microsoft.Extensions.Options;

namespace Fgs.Security.UserAuth;

public sealed class UserAuthProfileStore(
    ICacheService cache,
    IUserAuthProfileSource source,
    IOptions<UserAuthCacheOptions> options) : IUserAuthProfileStore
{
    private readonly UserAuthCacheOptions _options = options.Value;

    public async Task<UserAuthProfileDto?> GetOrLoadAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(entraObjectId))
        {
            return null;
        }

        var key = CacheKeys.UserAuthByEntraObjectId(entraObjectId);
        var cached = await cache.GetAsync<UserAuthProfileDto>(key, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        var profile = await source.LoadByEntraObjectIdAsync(entraObjectId, cancellationToken);
        if (profile is not null)
        {
            await SetAsync(profile, cancellationToken);
        }

        return profile;
    }

    public async Task SetAsync(
        UserAuthProfileDto profile,
        CancellationToken cancellationToken = default)
    {
        var expiration = TimeSpan.FromMinutes(_options.AbsoluteExpirationMinutes);
        await cache.SetAsync(
            CacheKeys.UserAuthByUserId(profile.UserId),
            profile,
            expiration,
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(profile.EntraObjectId))
        {
            await cache.SetAsync(
                CacheKeys.UserAuthByEntraObjectId(profile.EntraObjectId),
                profile,
                expiration,
                cancellationToken);
        }
    }

    public async Task InvalidateAsync(
        Guid userId,
        string? entraObjectId = null,
        CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(CacheKeys.UserAuthByUserId(userId), cancellationToken);

        if (!string.IsNullOrWhiteSpace(entraObjectId))
        {
            await cache.RemoveAsync(CacheKeys.UserAuthByEntraObjectId(entraObjectId), cancellationToken);
        }
    }
}
