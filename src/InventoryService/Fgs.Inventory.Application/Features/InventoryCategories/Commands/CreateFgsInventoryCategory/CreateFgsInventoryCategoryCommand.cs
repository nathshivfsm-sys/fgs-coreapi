using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Commands.CreateFgsInventoryCategory;

public sealed record CreateFgsInventoryCategoryCommand(FgsInventoryCategoryCreateDto Dto)
    : IRequest<ApiResponse<FgsInventoryCategoryDetailDto>>;
