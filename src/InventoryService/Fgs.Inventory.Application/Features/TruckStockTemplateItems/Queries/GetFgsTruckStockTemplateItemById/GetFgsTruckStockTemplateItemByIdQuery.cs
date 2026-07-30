using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Queries.GetFgsTruckStockTemplateItemById;

public sealed record GetFgsTruckStockTemplateItemByIdQuery(long TemplateId, long ItemId)
    : IRequest<ApiResponse<FgsTruckStockTemplateItemDetailDto>>;
