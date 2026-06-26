using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.ListActiveBillingCategories;

public sealed record ListActiveBillingCategoriesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, BillingCategoryListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<BillingCategorySummaryDto>>>;
