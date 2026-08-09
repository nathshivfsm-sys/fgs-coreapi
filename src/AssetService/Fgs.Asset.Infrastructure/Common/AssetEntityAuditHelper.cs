using Fgs.Foundation.Time;
using Fgs.Asset.Domain.Entities;
using Fgs.Kernel.Entities;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;

namespace Fgs.Asset.Infrastructure.Common;

public sealed class AssetEntityAuditHelper
{
    private readonly IFgsUserContext _userContext;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AssetEntityAuditHelper(
        IFgsUserContext userContext,
        ITenantContextAccessor tenantContextAccessor,
        IDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _tenantContextAccessor = tenantContextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public void StampForCreate(FgsAssetType entity) => StampActiveEntity(entity);
    public void StampForUpdate(FgsAssetType entity) => StampUpdate(entity);

    public void StampForCreate(FgsAssetManufacturer entity) => StampActiveEntity(entity);
    public void StampForUpdate(FgsAssetManufacturer entity) => StampUpdate(entity);

    public void StampForCreate(FgsAssetStatus entity) => StampActiveEntity(entity);
    public void StampForUpdate(FgsAssetStatus entity) => StampUpdate(entity);

    public void StampForCreate(FgsAssetModel entity) => StampActiveEntity(entity);
    public void StampForUpdate(FgsAssetModel entity) => StampUpdate(entity);

    public void StampForCreate(FgsAssetAttribute entity) => StampActiveEntity(entity);
    public void StampForUpdate(FgsAssetAttribute entity) => StampUpdate(entity);

    public void StampForCreate(FgsAssetAttributeOption entity) => StampActiveEntity(entity);
    public void StampForUpdate(FgsAssetAttributeOption entity) => StampUpdate(entity);

    public void StampForCreate(Domain.Entities.FgsAsset entity) => StampActiveEntity(entity);
    public void StampForUpdate(Domain.Entities.FgsAsset entity) => StampUpdate(entity);

    public void StampForCreate(FgsAssetWarranty entity) => StampEntity(entity);
    public void StampForUpdate(FgsAssetWarranty entity) => StampUpdate(entity);

    public void StampForCreate(FgsAssetAttributeValue entity) => StampEntity(entity);
    public void StampForUpdate(FgsAssetAttributeValue entity) => StampUpdate(entity);

    private void StampActiveEntity(FgsAssetType entity)
    {
        StampEntity(entity);
        entity.IsActive = true;
    }

    private void StampActiveEntity(FgsAssetManufacturer entity)
    {
        StampEntity(entity);
        entity.IsActive = true;
    }

    private void StampActiveEntity(FgsAssetStatus entity)
    {
        StampEntity(entity);
        entity.IsActive = true;
    }

    private void StampActiveEntity(FgsAssetModel entity)
    {
        StampEntity(entity);
        entity.IsActive = true;
    }

    private void StampActiveEntity(FgsAssetAttribute entity)
    {
        StampEntity(entity);
        entity.IsActive = true;
    }

    private void StampActiveEntity(FgsAssetAttributeOption entity)
    {
        StampEntity(entity);
        entity.IsActive = true;
    }

    private void StampActiveEntity(Domain.Entities.FgsAsset entity)
    {
        StampEntity(entity);
        entity.IsActive = true;
    }

    private void StampEntity(FgsEntityBase entity)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();
        var (tenantId, companyId) = ResolveTenantCompany();

        entity.CreatedOn = now;
        entity.CreatedBy = actor;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actor;

        switch (entity)
        {
            case FgsAssetType assetType:
                assetType.TenantId = tenantId;
                assetType.CompanyId = companyId;
                break;
            case FgsAssetManufacturer manufacturer:
                manufacturer.TenantId = tenantId;
                manufacturer.CompanyId = companyId;
                break;
            case FgsAssetStatus status:
                status.TenantId = tenantId;
                status.CompanyId = companyId;
                break;
            case FgsAssetModel model:
                model.TenantId = tenantId;
                model.CompanyId = companyId;
                break;
            case FgsAssetAttribute attribute:
                attribute.TenantId = tenantId;
                attribute.CompanyId = companyId;
                break;
            case FgsAssetAttributeOption option:
                option.TenantId = tenantId;
                option.CompanyId = companyId;
                break;
            case Domain.Entities.FgsAsset asset:
                asset.TenantId = tenantId;
                asset.CompanyId = companyId;
                break;
            case FgsAssetWarranty warranty:
                warranty.TenantId = tenantId;
                warranty.CompanyId = companyId;
                break;
            case FgsAssetAttributeValue attributeValue:
                attributeValue.TenantId = tenantId;
                attributeValue.CompanyId = companyId;
                break;
        }
    }

    private void StampUpdate(FgsEntityBase entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string ResolveActor() => _userContext.ResolveAuditActor();

    private (long TenantId, long CompanyId) ResolveTenantCompany()
    {
        if (_userContext.TenantId is long userTenantId && _userContext.CompanyId is long userCompanyId)
        {
            return (userTenantId, userCompanyId);
        }

        if (_tenantContextAccessor.Current is ITenantContext context)
        {
            return (context.TenantId, context.CompanyId);
        }

        throw new InvalidOperationException("Tenant context is required.");
    }
}
