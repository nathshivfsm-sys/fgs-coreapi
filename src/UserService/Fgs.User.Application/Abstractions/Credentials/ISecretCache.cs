namespace Fgs.User.Application.Abstractions.Credentials;

public interface ISecretCache
{
    bool TryGet(string cacheKey, out string secretJson);

    void Set(string cacheKey, string secretJson);

    void Invalidate(long tenantId, long companyId, Guid secretId);
}
