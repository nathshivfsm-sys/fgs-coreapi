using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.LookupTruckStockTemplates;

public sealed record LookupTruckStockTemplatesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsTruckStockTemplateLookupDto>>>;
