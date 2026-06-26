namespace Fgs.Foundation.Paging;

/// <summary>
/// Shared pagination, sorting, and search parameters for list endpoints.
/// </summary>
public sealed record PagedQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null,
    bool? IsActive = true);
