namespace Fgs.File.Application.Abstractions.Storage;

public interface IS3ObjectKeyBuilder
{
    string BuildCompanyAssetKey(
        long companyId,
        string entityType,
        long entityId,
        string fileName);

    string BuildThumbnailKey(string originalObjectKey);

    string TenantAssetsPrefix { get; }

    string CompanyAssetsPrefix(long companyId);
}
