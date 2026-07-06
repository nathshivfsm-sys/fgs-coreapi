using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Queries.ListActiveVendors;

public sealed record ListActiveVendorsQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null,
    FgsVendorListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsVendorSummaryDto>>>;
