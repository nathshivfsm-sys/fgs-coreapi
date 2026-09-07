using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Queries.ListInventoryCategoriesWithSubCategories;

public sealed class ListInventoryCategoriesWithSubCategoriesQueryHandler(
    IFgsInventoryCategoryReadRepository readRepository)
    : IRequestHandler<ListInventoryCategoriesWithSubCategoriesQuery, ApiResponse<PagedResult<FgsInventoryCategoryWithSubCategoriesDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInventoryCategoryWithSubCategoriesDto>>> Handle(
        ListInventoryCategoriesWithSubCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListWithSubCategoriesAsync(
            request.Query,
            request.Filters,
            cancellationToken);
        return ApiResponse<PagedResult<FgsInventoryCategoryWithSubCategoriesDto>>.Ok(result);
    }
}
