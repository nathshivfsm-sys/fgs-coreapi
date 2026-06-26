using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Queries.ListTechTrades;

public sealed class ListTechTradesQueryHandler(ITechTradeReadRepository readRepository)
    : IRequestHandler<ListTechTradesQuery, ApiResponse<PagedResult<TechTradeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<TechTradeSummaryDto>>> Handle(
        ListTechTradesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<TechTradeSummaryDto>>.Ok(result);
    }
}
