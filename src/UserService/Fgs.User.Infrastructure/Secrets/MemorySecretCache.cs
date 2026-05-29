using Fgs.User.Application.Abstractions.Credentials;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Fgs.User.Infrastructure.Common.Options;

namespace Fgs.User.Infrastructure.Secrets;

public sealed class MemorySecretCache : ISecretCache
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _ttl;

    public MemorySecretCache(IMemoryCache cache, IOptions<AwsCredentialsOptions> options)
    {
        _cache = cache;
        _ttl = TimeSpan.FromSeconds(Math.Max(1, options.Value.CacheTtlSeconds));
    }

    public static string BuildCacheKey(long tenantId, long companyId, Guid secretId, int versionNo) =>
        $"credential-secret:{tenantId}:{companyId}:{secretId}:{versionNo}";

    public bool TryGet(string cacheKey, out string secretJson) =>
        _cache.TryGetValue(cacheKey, out secretJson!);

    public void Set(string cacheKey, string secretJson) =>
        _cache.Set(cacheKey, secretJson, _ttl);

    public void Invalidate(long tenantId, long companyId, Guid secretId)
    {
        for (var version = 1; version <= 100; version++)
        {
            _cache.Remove(BuildCacheKey(tenantId, companyId, secretId, version));
        }
    }
}
