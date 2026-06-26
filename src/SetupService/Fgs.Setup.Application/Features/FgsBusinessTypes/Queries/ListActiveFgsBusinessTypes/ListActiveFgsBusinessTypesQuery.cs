using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.ListActiveFgsBusinessTypes;

public sealed record ListActiveFgsBusinessTypesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsBusinessTypeListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>>;
