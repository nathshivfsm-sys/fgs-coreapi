using Fgs.Foundation.Paging;

namespace Fgs.Scheduling.Application.Common.SchedulingCrud;

public sealed record SchedulingListQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null)
{
    public PagedQuery ToPagedQuery() =>
        new(Page, PageSize, SortBy, SortDirection, Search, IsActive: null);
}
