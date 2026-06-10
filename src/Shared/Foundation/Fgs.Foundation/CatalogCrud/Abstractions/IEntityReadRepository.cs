namespace Fgs.Foundation.CatalogCrud.Abstractions;

public interface IEntityReadRepository
{
    Task<object?> GetByIdAsync(
        CatalogEntityDescriptor descriptor,
        string id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<object>> ListAsync(
        CatalogEntityDescriptor descriptor,
        PagedQuery paging,
        IReadOnlyDictionary<string, string?> filters,
        Type summaryDtoType,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        CatalogEntityDescriptor descriptor,
        IReadOnlyDictionary<string, object?> propertyValues,
        string? excludeId,
        CancellationToken cancellationToken = default);
}
