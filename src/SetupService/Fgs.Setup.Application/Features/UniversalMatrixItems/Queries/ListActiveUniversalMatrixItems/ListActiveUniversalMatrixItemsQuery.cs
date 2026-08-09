using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Queries.ListActiveUniversalMatrixItems;

public sealed record ListActiveUniversalMatrixItemsQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsUniversalMatrixItemListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixItemSummaryDto>>>;
