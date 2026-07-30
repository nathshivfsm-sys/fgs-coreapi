using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.CreateFgsTruckStockTemplateItem;

public sealed record CreateFgsTruckStockTemplateItemCommand(long TemplateId, FgsTruckStockTemplateItemCreateDto Dto)
    : IRequest<ApiResponse<FgsTruckStockTemplateItemDetailDto>>;
