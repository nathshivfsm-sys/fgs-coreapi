using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Queries.ListInventoryCategories;

public sealed record ListInventoryCategoriesQuery(
    InventoryListQuery Query,
    FgsInventoryCategoryListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInventoryCategorySummaryDto>>>;
