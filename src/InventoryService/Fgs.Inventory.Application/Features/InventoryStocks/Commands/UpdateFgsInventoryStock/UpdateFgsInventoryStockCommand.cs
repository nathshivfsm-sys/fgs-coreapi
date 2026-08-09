using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Commands.UpdateFgsInventoryStock;

public sealed record UpdateFgsInventoryStockCommand(long Id, FgsInventoryStockUpdateDto Dto)
    : IRequest<ApiResponse<FgsInventoryStockDetailDto>>;
