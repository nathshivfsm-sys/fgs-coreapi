using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Application.Common.InventoryCrud;

public sealed record InventoryListQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null,
    bool? IsActive = true)
{
    public PagedQuery ToPagedQuery() =>
        new(Page, PageSize, SortBy, SortDirection, Search, IsActive);
}
