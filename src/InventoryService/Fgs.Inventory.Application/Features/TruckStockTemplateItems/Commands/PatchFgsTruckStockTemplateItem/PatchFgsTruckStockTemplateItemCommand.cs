using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.PatchFgsTruckStockTemplateItem;

public sealed record PatchFgsTruckStockTemplateItemCommand(long TemplateId, long ItemId, FgsTruckStockTemplateItemPatchDto Dto)
    : IRequest<ApiResponse<FgsTruckStockTemplateItemDetailDto>>;
