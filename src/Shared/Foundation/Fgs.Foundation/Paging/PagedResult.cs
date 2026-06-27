namespace Fgs.Foundation.Paging;

/// <summary>
/// Paginated result wrapper for list endpoints.
/// </summary>
public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount);
