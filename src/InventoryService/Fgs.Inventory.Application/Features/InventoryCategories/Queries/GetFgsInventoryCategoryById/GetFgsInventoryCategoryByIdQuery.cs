using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Queries.GetFgsInventoryCategoryById;

public sealed record GetFgsInventoryCategoryByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventoryCategoryDetailDto>>;
