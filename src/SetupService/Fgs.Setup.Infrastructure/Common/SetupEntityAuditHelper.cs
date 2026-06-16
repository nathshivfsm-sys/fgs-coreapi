using Fgs.Kernel.Entities;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Abstractions.Time;

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
