using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.GetFgsTruckStockTemplateById;

public sealed record GetFgsTruckStockTemplateByIdQuery(long Id)
    : IRequest<ApiResponse<FgsTruckStockTemplateDetailDto>>;
