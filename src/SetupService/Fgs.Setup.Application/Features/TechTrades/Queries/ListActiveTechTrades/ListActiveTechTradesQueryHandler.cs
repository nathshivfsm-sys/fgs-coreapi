using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Queries.ListActiveTechTrades;

public sealed class ListActiveTechTradesQueryHandler(ITechTradeReadRepository readRepository)
    : IRequestHandler<ListActiveTechTradesQuery, ApiResponse<PagedResult<TechTradeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<TechTradeSummaryDto>>> Handle(
        ListActiveTechTradesQuery request,
        CancellationToken cancellationToken)
    {
        var query = new SetupListQuery(
            request.Page,
            request.PageSize,
            request.SortBy,
            request.SortDirection,
            request.Search,
            IsActive: true);

        var result = await readRepository.ListAsync(
            query,
            request.Filters ?? new TechTradeListFilters(),
            cancellationToken);

        return ApiResponse<PagedResult<TechTradeSummaryDto>>.Ok(result);
    }
}
