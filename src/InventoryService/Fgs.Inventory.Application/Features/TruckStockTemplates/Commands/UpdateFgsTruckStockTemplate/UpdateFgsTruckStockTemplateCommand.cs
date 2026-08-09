using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.UpdateFgsTruckStockTemplate;

public sealed record UpdateFgsTruckStockTemplateCommand(long Id, FgsTruckStockTemplateUpdateDto Dto)
    : IRequest<ApiResponse<FgsTruckStockTemplateDetailDto>>;
