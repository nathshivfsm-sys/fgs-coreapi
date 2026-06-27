using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.ListActiveSalesActivityOutcomes;

public sealed record ListActiveSalesActivityOutcomesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSalesActivityOutcomeListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>>;
