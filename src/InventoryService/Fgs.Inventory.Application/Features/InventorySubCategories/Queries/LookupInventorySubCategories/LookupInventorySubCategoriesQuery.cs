using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Queries.LookupInventorySubCategories;

public sealed record LookupInventorySubCategoriesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventorySubCategoryLookupDto>>>;
