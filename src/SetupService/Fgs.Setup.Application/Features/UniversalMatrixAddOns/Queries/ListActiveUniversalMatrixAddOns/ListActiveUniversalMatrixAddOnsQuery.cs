using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.ListActiveUniversalMatrixAddOns;

public sealed record ListActiveUniversalMatrixAddOnsQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsUniversalMatrixAddOnListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixAddOnSummaryDto>>>;
