using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.UpdateFgsTruckStockTemplateItem;

public sealed record UpdateFgsTruckStockTemplateItemCommand(long TemplateId, long ItemId, FgsTruckStockTemplateItemUpdateDto Dto)
    : IRequest<ApiResponse<FgsTruckStockTemplateItemDetailDto>>;
