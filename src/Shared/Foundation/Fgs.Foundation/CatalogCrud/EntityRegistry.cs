namespace Fgs.Foundation.CatalogCrud;

public sealed class EntityRegistry : Abstractions.IEntityRegistry
{
    private readonly Dictionary<string, CatalogEntityDescriptor> _descriptors = new(StringComparer.OrdinalIgnoreCase);

    public void Register(CatalogEntityDescriptor descriptor) =>
        _descriptors[descriptor.Key] = descriptor;

    public CatalogEntityDescriptor GetRequired(string key) =>
        _descriptors.TryGetValue(key, out var descriptor)
            ? descriptor
            : throw new KeyNotFoundException($"Catalog entity '{key}' is not registered.");

    public bool TryGet(string key, out CatalogEntityDescriptor descriptor) =>
        _descriptors.TryGetValue(key, out descriptor!);

    public IReadOnlyCollection<CatalogEntityDescriptor> All => _descriptors.Values;
}
