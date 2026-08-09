using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Commands.UpdateFgsInventorySubCategory;

public sealed record UpdateFgsInventorySubCategoryCommand(long Id, FgsInventorySubCategoryUpdateDto Dto)
    : IRequest<ApiResponse<FgsInventorySubCategoryDetailDto>>;
