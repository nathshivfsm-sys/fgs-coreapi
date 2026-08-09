using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.PatchFgsTruckStockTemplate;

public sealed record PatchFgsTruckStockTemplateCommand(long Id, FgsTruckStockTemplatePatchDto Dto)
    : IRequest<ApiResponse<FgsTruckStockTemplateDetailDto>>;
