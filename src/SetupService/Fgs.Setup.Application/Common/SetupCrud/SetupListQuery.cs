using Fgs.Foundation.Paging;

namespace Fgs.Setup.Application.Common.SetupCrud;

public sealed record SetupListQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null,
    bool? IsActive = null)
{
    public PagedQuery ToPagedQuery() =>
        new(Page, PageSize, SortBy, SortDirection, Search, IsActive);
}
