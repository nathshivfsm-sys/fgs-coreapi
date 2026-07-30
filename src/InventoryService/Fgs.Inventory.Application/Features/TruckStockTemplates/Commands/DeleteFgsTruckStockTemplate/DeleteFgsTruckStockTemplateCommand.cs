using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.DeleteFgsTruckStockTemplate;

public sealed record DeleteFgsTruckStockTemplateCommand(long Id)
    : IRequest<ApiResponse<FgsTruckStockTemplateDetailDto>>;
