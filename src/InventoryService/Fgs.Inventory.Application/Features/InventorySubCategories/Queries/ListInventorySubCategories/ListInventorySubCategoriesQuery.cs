using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Queries.ListInventorySubCategories;

public sealed record ListInventorySubCategoriesQuery(
    InventoryListQuery Query,
    FgsInventorySubCategoryListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInventorySubCategorySummaryDto>>>;
