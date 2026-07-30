using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.CreateFgsTruckStockTemplate;

public sealed record CreateFgsTruckStockTemplateCommand(FgsTruckStockTemplateCreateDto Dto)
    : IRequest<ApiResponse<FgsTruckStockTemplateDetailDto>>;
