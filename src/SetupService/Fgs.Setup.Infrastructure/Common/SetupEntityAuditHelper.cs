using Fgs.Kernel.Entities;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Abstractions.Time;
using Fgs.Setup.Domain.Entities;

namespace Fgs.Setup.Infrastructure.Common;

public sealed class SetupEntityAuditHelper
{
    private readonly IFgsUserContext _userContext;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SetupEntityAuditHelper(
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

    public void StampForCreate(FgsLocation location)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();
        var (tenantId, companyId) = ResolveTenantCompany();

        location.CreatedOn = now;
        location.CreatedBy = actor;
        location.UpdatedOn = now;
        location.UpdatedBy = actor;
        location.IsActive = true;
        location.TenantId = tenantId;
        location.CompanyId = companyId;
    }

    public void StampForUpdate(FgsLocation location)
    {
        location.UpdatedOn = _dateTimeProvider.UtcNow;
        location.UpdatedBy = ResolveActor();
    }

    public void StampForCreate(FgsSetupGLBreakTrade trade, long glBreakId)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();
        var (tenantId, companyId) = ResolveTenantCompany();

        trade.GLBreakId = glBreakId;
        trade.TenantId = tenantId;
        trade.CompanyId = companyId;
        trade.CreatedOn = now;
        trade.CreatedBy = actor;
    }

    private string ResolveActor() =>
        _userContext.UserId?.ToString()
        ?? _userContext.Email
        ?? "System";

    private (long TenantId, long CompanyId) ResolveTenantCompany()
    {
        if (_userContext.TenantId is long userTenantId && _userContext.CompanyId is long userCompanyId)
        {
            return (userTenantId, userCompanyId);
        }

        if (_tenantContextAccessor.Current is { IsResolved: true } context)
        {
            return (context.TenantId, context.CompanyId);
        }

        throw new InvalidOperationException("Tenant context is required.");
    }
}
