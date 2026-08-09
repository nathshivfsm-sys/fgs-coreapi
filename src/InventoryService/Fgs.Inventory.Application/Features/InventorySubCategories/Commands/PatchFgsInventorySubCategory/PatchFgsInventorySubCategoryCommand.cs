using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Commands.PatchFgsInventorySubCategory;

public sealed record PatchFgsInventorySubCategoryCommand(long Id, FgsInventorySubCategoryPatchDto Dto)
    : IRequest<ApiResponse<FgsInventorySubCategoryDetailDto>>;
