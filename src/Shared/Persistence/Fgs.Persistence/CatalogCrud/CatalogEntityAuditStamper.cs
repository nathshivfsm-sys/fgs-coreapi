using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Security.Abstractions;

namespace Fgs.Persistence.CatalogCrud;

public sealed class CatalogEntityAuditStamper : IEntityAuditStamper
{
    private readonly IFgsUserContext _userContext;
    private readonly ICatalogDateTimeProvider _dateTimeProvider;

    public CatalogEntityAuditStamper(IFgsUserContext userContext, ICatalogDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public void StampForCreate(object entity, CatalogEntityDescriptor descriptor)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();

        SetProperty(entity, "CreatedOn", now);
        SetProperty(entity, "CreatedBy", actor);
        SetProperty(entity, "UpdatedOn", now);
        SetProperty(entity, "UpdatedBy", actor);

        if (descriptor.Variant is CatalogEntityVariant.StandardLong or CatalogEntityVariant.StandardGuid
            or CatalogEntityVariant.ScopedManualAudit or CatalogEntityVariant.NullableTenantScope)
        {
            SetProperty(entity, "TenantId", _userContext.TenantId);
            SetProperty(entity, "CompanyId", _userContext.CompanyId);
        }

        if (descriptor.SupportsSoftDelete)
        {
            SetProperty(entity, "IsActive", true);
        }
    }

    public void StampForUpdate(object entity, CatalogEntityDescriptor descriptor)
    {
        SetProperty(entity, "UpdatedOn", _dateTimeProvider.UtcNow);
        SetProperty(entity, "UpdatedBy", ResolveActor());
    }

    private string ResolveActor() =>
        _userContext.UserId?.ToString()
        ?? _userContext.Email
        ?? "System";

    private static void SetProperty(object entity, string propertyName, object? value)
    {
        var property = entity.GetType().GetProperty(propertyName);
        if (property is null || !property.CanWrite || value is null)
        {
            return;
        }

        if (property.PropertyType == typeof(long?) && value is long longValue)
        {
            property.SetValue(entity, longValue);
            return;
        }

        if (property.PropertyType == typeof(long) && value is long requiredLong)
        {
            property.SetValue(entity, requiredLong);
            return;
        }

        if (property.PropertyType.IsAssignableFrom(value.GetType()))
        {
            property.SetValue(entity, value);
        }
    }
}
