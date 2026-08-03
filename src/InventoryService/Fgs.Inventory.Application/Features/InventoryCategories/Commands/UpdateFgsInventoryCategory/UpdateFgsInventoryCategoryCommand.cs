using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Commands.UpdateFgsInventoryCategory;

public sealed record UpdateFgsInventoryCategoryCommand(long Id, FgsInventoryCategoryUpdateDto Dto)
    : IRequest<ApiResponse<FgsInventoryCategoryDetailDto>>;
