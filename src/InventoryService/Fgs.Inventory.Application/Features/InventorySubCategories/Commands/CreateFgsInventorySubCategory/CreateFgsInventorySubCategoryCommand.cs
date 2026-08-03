using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Commands.CreateFgsInventorySubCategory;

public sealed record CreateFgsInventorySubCategoryCommand(FgsInventorySubCategoryCreateDto Dto)
    : IRequest<ApiResponse<FgsInventorySubCategoryDetailDto>>;
