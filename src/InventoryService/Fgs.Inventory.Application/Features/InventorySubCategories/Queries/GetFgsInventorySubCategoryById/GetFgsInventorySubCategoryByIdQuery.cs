using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Queries.GetFgsInventorySubCategoryById;

public sealed record GetFgsInventorySubCategoryByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventorySubCategoryDetailDto>>;
