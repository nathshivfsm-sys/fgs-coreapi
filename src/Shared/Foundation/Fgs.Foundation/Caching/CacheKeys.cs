using System.Security.Cryptography;
using System.Text;

namespace Fgs.Foundation.Caching;

public static class CacheKeys
{
    public static string Build(long tenantId, long companyId, string entity, string idSegment) =>
        $"tenant:{tenantId}:company:{companyId}:{entity}:{idSegment}";

    public static string EntityPrefix(long tenantId, long companyId, string entity) =>
        $"tenant:{tenantId}:company:{companyId}:{entity}:";

    public static string LookupSegment(bool activeOnly) =>
        $"lookup:activeOnly={activeOnly.ToString().ToLowerInvariant()}";

    public static string ListActiveSegment(
        int page,
        int pageSize,
        string? sortBy,
        string sortDirection,
        string? search,
        string? filtersFingerprint = null)
    {
        var raw = string.Join(
            '|',
            page,
            pageSize,
            sortBy ?? string.Empty,
            sortDirection,
            search ?? string.Empty,
            filtersFingerprint ?? string.Empty);

        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)))[..16].ToLowerInvariant();
        return $"list-active:{hash}";
    }

    public static string Fingerprint(object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var json = CacheJsonSerializer.Serialize(value);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(json)))[..16].ToLowerInvariant();
    }

    public static string UserAuthByEntraObjectId(string entraObjectId) =>
        $"user:auth:oid:{entraObjectId.ToLowerInvariant()}";

    public static string UserAuthByUserId(Guid userId) =>
        $"user:auth:id:{userId:D}";

    public static string LoginPkceByState(string state) =>
        $"login:pkce:{state}";
}
