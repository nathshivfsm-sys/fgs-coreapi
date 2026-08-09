using Fgs.MultiTenancy;
using Fgs.Foundation.Time;
using Fgs.Scheduling.Domain.Entities;
using Fgs.Security.Abstractions;

namespace Fgs.Scheduling.Infrastructure.Common;

public sealed class SchedulingEntityAuditHelper
{
    private readonly IFgsUserContext _userContext;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SchedulingEntityAuditHelper(
        IFgsUserContext userContext,
        ITenantContextAccessor tenantContextAccessor,
        IDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _tenantContextAccessor = tenantContextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public void StampForCreate(FgsAppointment entity)
    {
        var now = _dateTimeProvider.UtcNow;
        var actorId = ResolveNumericActor();
        var (tenantId, companyId) = ResolveTenantCompany();

        entity.CreatedOn = now;
        entity.CreatedBy = actorId;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actorId;
        entity.TenantId = tenantId;
        entity.CompanyId = companyId;
    }

    /// <summary>
    /// Appointment audit columns store numeric actor ids. Until identity exposes a long user key,
    /// fall back to 0 (system) when only a Guid user id is available.
    /// </summary>
    private static long ResolveNumericActor() => 0;

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
