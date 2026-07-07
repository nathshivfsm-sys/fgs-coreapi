using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.ListActiveUniversalMatrixSizeTiers;

public sealed record ListActiveUniversalMatrixSizeTiersQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsUniversalMatrixSizeTierListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>>>;
