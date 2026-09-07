using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Queries.ListInventoryCategoriesWithSubCategories;

public sealed record ListInventoryCategoriesWithSubCategoriesQuery(
    InventoryListQuery Query,
    FgsInventoryCategoryListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInventoryCategoryWithSubCategoriesDto>>>;
