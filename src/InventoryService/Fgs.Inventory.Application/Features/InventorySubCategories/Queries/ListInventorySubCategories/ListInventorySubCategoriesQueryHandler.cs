using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Queries.ListInventorySubCategories;

public sealed class ListInventorySubCategoriesQueryHandler(IFgsInventorySubCategoryReadRepository readRepository)
    : IRequestHandler<ListInventorySubCategoriesQuery, ApiResponse<PagedResult<FgsInventorySubCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInventorySubCategorySummaryDto>>> Handle(
        ListInventorySubCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsInventorySubCategorySummaryDto>>.Ok(result);
    }
}
