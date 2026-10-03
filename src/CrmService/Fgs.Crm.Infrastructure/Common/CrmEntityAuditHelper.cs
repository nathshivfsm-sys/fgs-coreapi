using Fgs.Crm.Domain.Entities;
using Fgs.Kernel.Entities;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using Fgs.Foundation.Time;

namespace Fgs.Crm.Infrastructure.Common;

public sealed class CrmEntityAuditHelper
{
    private readonly IFgsUserContext _userContext;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CrmEntityAuditHelper(
        IFgsUserContext userContext,
        ITenantContextAccessor tenantContextAccessor,
        IDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _tenantContextAccessor = tenantContextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public void StampForCreate(CrmCustomer entity)
    {
        StampAudit(entity, (tenantId, companyId) =>
        {
            entity.TenantId = tenantId;
            entity.CompanyId = companyId;
        });
        entity.IsActive = true;
    }

    public void StampForCreate(CrmServiceLocation entity)
    {
        StampAudit(entity, (tenantId, companyId) =>
        {
            entity.TenantId = tenantId;
            entity.CompanyId = companyId;
        });
        entity.IsActive = true;
    }

    public void StampForCreate(CrmContact entity)
    {
        StampAudit(entity, (tenantId, companyId) =>
        {
            entity.TenantId = tenantId;
            entity.CompanyId = companyId;
        });
        entity.IsActive = true;
    }

    public void StampForCreate(CrmEntityTag entity) =>
        StampAudit(entity, (tenantId, companyId) =>
        {
            entity.TenantId = tenantId;
            entity.CompanyId = companyId;
        });

    public void StampForCreate(CrmContactCommunication entity) =>
        StampAudit(entity, (tenantId, companyId) =>
        {
            entity.TenantId = tenantId;
            entity.CompanyId = companyId;
        });

    private void StampAudit(FgsEntityBase entity, Action<long, long> assignScope)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();
        var (tenantId, companyId) = ResolveTenantCompany();

        entity.CreatedOn = now;
        entity.CreatedBy = actor;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actor;
        assignScope(tenantId, companyId);
    }

    public void StampForUpdate(CrmCustomer entity)
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
