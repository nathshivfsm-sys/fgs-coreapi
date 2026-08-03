using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Queries.GetFgsInventoryStockById;

public sealed record GetFgsInventoryStockByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventoryStockDetailDto>>;
