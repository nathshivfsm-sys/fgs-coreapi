using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Commands.CreateFgsInventoryStock;

public sealed record CreateFgsInventoryStockCommand(FgsInventoryStockCreateDto Dto)
    : IRequest<ApiResponse<FgsInventoryStockDetailDto>>;
