using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Commands.PatchFgsInventoryCategory;

public sealed record PatchFgsInventoryCategoryCommand(long Id, FgsInventoryCategoryPatchDto Dto)
    : IRequest<ApiResponse<FgsInventoryCategoryDetailDto>>;
