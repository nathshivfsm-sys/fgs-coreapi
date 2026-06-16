using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Queries.ListActiveTechTrades;

public sealed record ListActiveTechTradesQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null,
    TechTradeListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<TechTradeSummaryDto>>>;
