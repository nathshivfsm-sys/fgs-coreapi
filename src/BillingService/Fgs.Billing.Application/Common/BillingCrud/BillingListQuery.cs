using Fgs.Foundation.Paging;

namespace Fgs.Billing.Application.Common.BillingCrud;

public sealed record BillingListQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null)
{
    public PagedQuery ToPagedQuery() =>
        new(Page, PageSize, SortBy, SortDirection, Search, IsActive: null);
}
