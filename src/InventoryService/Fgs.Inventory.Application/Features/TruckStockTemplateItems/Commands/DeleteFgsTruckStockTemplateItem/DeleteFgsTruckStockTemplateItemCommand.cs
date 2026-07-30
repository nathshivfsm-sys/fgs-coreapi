using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.DeleteFgsTruckStockTemplateItem;

public sealed record DeleteFgsTruckStockTemplateItemCommand(long TemplateId, long ItemId)
    : IRequest<ApiResponse<object>>;
