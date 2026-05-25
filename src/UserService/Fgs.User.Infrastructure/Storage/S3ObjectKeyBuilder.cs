using Fgs.User.Application.Abstractions.Storage;

namespace Fgs.User.Infrastructure.Storage;

public sealed class S3ObjectKeyBuilder : IS3ObjectKeyBuilder
{
    public const string TenantAssetsRoot = "tenant-assets/";

    public string TenantAssetsPrefix => TenantAssetsRoot;

    public string CompanyAssetsPrefix(long companyId) =>
        $"company-assets/{companyId}/";

    public string BuildCompanyAssetKey(
        long companyId,
        string entityType,
        long entityId,
        string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(entityType);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var normalizedEntityType = entityType.Trim().Trim('/');
        return $"{CompanyAssetsPrefix(companyId)}{normalizedEntityType}/{entityId}/{fileName.TrimStart('/')}";
    }

    public string BuildThumbnailKey(string originalObjectKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(originalObjectKey);

        var lastDot = originalObjectKey.LastIndexOf('.');
        if (lastDot < 0)
        {
            return $"{originalObjectKey}-thumb";
        }

        var nameWithoutExtension = originalObjectKey[..lastDot];
        var extension = originalObjectKey[lastDot..];
        return $"{nameWithoutExtension}-thumb{extension}";
    }
}
