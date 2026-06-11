namespace Fgs.Foundation.CatalogCrud.Abstractions;

public interface IEntityAuditStamper
{
    void StampForCreate(object entity, CatalogEntityDescriptor descriptor);

    void StampForUpdate(object entity, CatalogEntityDescriptor descriptor);
}
