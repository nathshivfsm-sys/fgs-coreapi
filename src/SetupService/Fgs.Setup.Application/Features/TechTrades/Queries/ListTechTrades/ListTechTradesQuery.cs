using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Queries.ListTechTrades;

public sealed record ListTechTradesQuery(
    SetupListQuery Query,
    TechTradeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<TechTradeSummaryDto>>>;
