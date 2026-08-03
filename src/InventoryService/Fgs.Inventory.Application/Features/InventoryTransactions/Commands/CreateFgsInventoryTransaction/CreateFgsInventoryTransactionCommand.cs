using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryTransactions.Commands.CreateFgsInventoryTransaction;

public sealed record CreateFgsInventoryTransactionCommand(FgsInventoryTransactionCreateDto Dto)
    : IRequest<ApiResponse<FgsInventoryTransactionDetailDto>>;
