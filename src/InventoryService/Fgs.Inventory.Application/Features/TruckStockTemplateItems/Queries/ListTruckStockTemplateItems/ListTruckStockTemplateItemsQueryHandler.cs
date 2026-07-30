using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Queries.ListTruckStockTemplateItems;

public sealed class ListTruckStockTemplateItemsQueryHandler(IFgsTruckStockTemplateItemReadRepository readRepository)
    : IRequestHandler<ListTruckStockTemplateItemsQuery, ApiResponse<PagedResult<FgsTruckStockTemplateItemSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsTruckStockTemplateItemSummaryDto>>> Handle(
        ListTruckStockTemplateItemsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListByTemplateAsync(
            request.TemplateId,
            request.Query,
            request.Filters,
            cancellationToken);
        return ApiResponse<PagedResult<FgsTruckStockTemplateItemSummaryDto>>.Ok(result);
    }
}
