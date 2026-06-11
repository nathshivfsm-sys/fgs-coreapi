namespace Fgs.Foundation.CatalogCrud.Abstractions;

public interface IEntityWriteService
{
    Task<object> CreateAsync(
        CatalogEntityDescriptor descriptor,
        object createDto,
        CancellationToken cancellationToken = default);

    Task<object> UpdateAsync(
        CatalogEntityDescriptor descriptor,
        string id,
        object updateDto,
        CancellationToken cancellationToken = default);

    Task<object> PatchAsync(
        CatalogEntityDescriptor descriptor,
        string id,
        object patchDto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        CatalogEntityDescriptor descriptor,
        string id,
        CancellationToken cancellationToken = default);
}
