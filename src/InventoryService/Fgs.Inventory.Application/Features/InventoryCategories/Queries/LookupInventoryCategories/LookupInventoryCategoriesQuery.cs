using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Queries.LookupInventoryCategories;

public sealed record LookupInventoryCategoriesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryCategoryLookupDto>>>;
