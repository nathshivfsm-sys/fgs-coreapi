using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Queries.ListInventoryCategories;

public sealed class ListInventoryCategoriesQueryHandler(IFgsInventoryCategoryReadRepository readRepository)
    : IRequestHandler<ListInventoryCategoriesQuery, ApiResponse<PagedResult<FgsInventoryCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInventoryCategorySummaryDto>>> Handle(
        ListInventoryCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsInventoryCategorySummaryDto>>.Ok(result);
    }
}
