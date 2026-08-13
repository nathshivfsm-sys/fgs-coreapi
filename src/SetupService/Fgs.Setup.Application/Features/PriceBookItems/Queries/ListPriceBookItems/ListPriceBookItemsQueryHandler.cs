using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.PriceBookItems;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Queries.ListPriceBookItems;

public sealed class ListPriceBookItemsQueryHandler(IFgsPriceBookItemReadRepository readRepository)
    : IRequestHandler<ListPriceBookItemsQuery, ApiResponse<PagedResult<FgsPriceBookItemSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsPriceBookItemSummaryDto>>> Handle(
        ListPriceBookItemsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsPriceBookItemSummaryDto>>.Ok(result);
    }
}
