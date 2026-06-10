namespace Fgs.Foundation.CatalogCrud.Abstractions;

public interface IEntityRegistry
{
    void Register(CatalogEntityDescriptor descriptor);

    CatalogEntityDescriptor GetRequired(string key);

    bool TryGet(string key, out CatalogEntityDescriptor descriptor);

    IReadOnlyCollection<CatalogEntityDescriptor> All { get; }
}
