namespace Fgs.Foundation.CatalogCrud;

/// <summary>
/// Paginated result wrapper for catalog list endpoints.
/// </summary>
public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount);
