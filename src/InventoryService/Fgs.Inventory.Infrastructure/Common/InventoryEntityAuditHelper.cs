using System.Reflection;
using Fgs.Inventory.Domain.Entities;
using Fgs.Kernel.Entities;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using Fgs.Inventory.Application.Abstractions.Time;

namespace Fgs.Inventory.Infrastructure.Common;

public sealed class InventoryEntityAuditHelper
{
    private readonly IFgsUserContext _userContext;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public InventoryEntityAuditHelper(
        IFgsUserContext userContext,
        ITenantContextAccessor tenantContextAccessor,
        IDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _tenantContextAccessor = tenantContextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public void StampForCreate(FgsTenantCompanySetupEntityBase<long> entity)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();
        var (tenantId, companyId) = ResolveTenantCompany();

        entity.CreatedOn = now;
        entity.CreatedBy = actor;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actor;
        entity.IsActive = true;
        entity.TenantId = tenantId;
        entity.CompanyId = companyId;
    }

    public void StampForUpdate(FgsTenantCompanySetupEntityBase<long> entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    public void StampForCreate(FgsEntityBase entity, ITenantCompanyScoped scoped)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();

        entity.CreatedOn = now;
        entity.CreatedBy = actor;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actor;

        if (entity is ITenantCompanyScoped)
        {
            var (tenantId, companyId) = scoped.TenantId > 0 && scoped.CompanyId > 0
                ? (scoped.TenantId, scoped.CompanyId)
                : ResolveTenantCompany();

            SetTenantCompany(entity, tenantId, companyId);
        }
    }

    public void StampForUpdate(FgsEntityBase entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    public void StampForCreate(FgsTruckStockTemplateItem entity) =>
        StampForCreate(entity, entity);

    public void StampForUpdate(FgsTruckStockTemplateItem entity) =>
        StampForUpdate((FgsEntityBase)entity);

    public void StampStockUpdated(FgsInventoryStock stock) =>
        stock.UpdatedOn = _dateTimeProvider.UtcNow;

    public void StampForCreateStock(FgsInventoryStock stock)
    {
        var (tenantId, companyId) = ResolveTenantCompany();
        stock.TenantId = tenantId;
        stock.CompanyId = companyId;
        stock.UpdatedOn = _dateTimeProvider.UtcNow;
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

    private static void SetTenantCompany(FgsEntityBase entity, long tenantId, long companyId)
    {
        var type = entity.GetType();
        type.GetProperty(nameof(ITenantCompanyScoped.TenantId), BindingFlags.Public | BindingFlags.Instance)
            ?.SetValue(entity, tenantId);
        type.GetProperty(nameof(ITenantCompanyScoped.CompanyId), BindingFlags.Public | BindingFlags.Instance)
            ?.SetValue(entity, companyId);
    }
}
