using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Commands.PatchFgsInventoryStock;

public sealed record PatchFgsInventoryStockCommand(long Id, FgsInventoryStockPatchDto Dto)
    : IRequest<ApiResponse<FgsInventoryStockDetailDto>>;
