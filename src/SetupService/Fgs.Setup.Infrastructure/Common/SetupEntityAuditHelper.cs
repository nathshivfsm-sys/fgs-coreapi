using Fgs.Kernel.Entities;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
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


    public void StampForCreate(FgsLeadDisqualificationReason entity)
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

    public void StampForUpdate(FgsLeadDisqualificationReason entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    public void StampForCreate(FgsLeadSource entity)
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

    public void StampForUpdate(FgsLeadSource entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    public void StampForCreate(FgsLeadStatus entity)
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

    public void StampForUpdate(FgsLeadStatus entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }


    public void StampForCreate(FgsSalesPipelineStatus entity)
    {
        StampTenantCompanyEntity(entity);
    }

    public void StampForUpdate(FgsSalesPipelineStatus entity)
    {
        StampTenantCompanyUpdate(entity);
    }

    public void StampForCreate(FgsSalesActivityType entity)
    {
        StampTenantCompanyEntity(entity);
    }

    public void StampForUpdate(FgsSalesActivityType entity)
    {
        StampTenantCompanyUpdate(entity);
    }

    public void StampForCreate(FgsSalesDispositionReason entity)
    {
        StampTenantCompanyEntity(entity);
    }

    public void StampForUpdate(FgsSalesDispositionReason entity)
    {
        StampTenantCompanyUpdate(entity);
    }

    public void StampForCreate(FgsSalesActivityOutcome entity)
    {
        StampTenantCompanyEntity(entity);
    }

    public void StampForUpdate(FgsSalesActivityOutcome entity)
    {
        StampTenantCompanyUpdate(entity);
    }

    public void StampForCreate(FgsVehicleMaintenance entity)
    {
        StampTenantCompanyEntity(entity);
    }

    public void StampForUpdate(FgsVehicleMaintenance entity)
    {
        StampTenantCompanyUpdate(entity);
    }

    private void StampTenantCompanyEntity(FgsEntityBase entity)
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
            case FgsSalesPipelineStatus salesPipelineStatus:
                salesPipelineStatus.TenantId = tenantId;
                salesPipelineStatus.CompanyId = companyId;
                salesPipelineStatus.IsActive = true;
                break;
            case FgsSalesActivityType salesActivityType:
                salesActivityType.TenantId = tenantId;
                salesActivityType.CompanyId = companyId;
                salesActivityType.IsActive = true;
                break;
            case FgsSalesDispositionReason salesDispositionReason:
                salesDispositionReason.TenantId = tenantId;
                salesDispositionReason.CompanyId = companyId;
                salesDispositionReason.IsActive = true;
                break;
            case FgsSalesActivityOutcome salesActivityOutcome:
                salesActivityOutcome.TenantId = tenantId;
                salesActivityOutcome.CompanyId = companyId;
                salesActivityOutcome.IsActive = true;
                break;
            case FgsVehicleMaintenance vehicleMaintenance:
                vehicleMaintenance.TenantId = tenantId;
                vehicleMaintenance.CompanyId = companyId;
                vehicleMaintenance.IsActive = true;
                break;
        }
    }

    private void StampTenantCompanyUpdate(FgsEntityBase entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    public void StampForCreate(FgsTag entity)
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

    public void StampForUpdate(FgsTag entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    public void StampForCreate(FgsEmployee entity)
    {
        var now = _dateTimeProvider.UtcNow.DateTime;
        var actorId = ResolveNumericActor();
        var (tenantId, companyId) = ResolveTenantCompany();

        entity.CreatedOn = now;
        entity.CreatedBy = actorId;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actorId;
        entity.TenantId = tenantId;
        entity.CompanyId = companyId;
    }

    public void StampForUpdate(FgsEmployee entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow.DateTime;
        entity.UpdatedBy = ResolveNumericActor();
    }

    public void StampForCreate(FgsSetupCommunicationTemplate entity, long? tenantId, long? companyId)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();

        entity.TenantId = tenantId;
        entity.CompanyId = companyId;
        entity.CreatedOn = now;
        entity.CreatedBy = actor;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actor;
        entity.IsActive = true;
    }

    public void StampForUpdate(FgsSetupCommunicationTemplate entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string ResolveActor() => _userContext.ResolveAuditActor();

    /// <summary>
    /// Employee audit columns store numeric actor ids. Until identity exposes a long user key,
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
