using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryTransactions.Queries.GetFgsInventoryTransactionById;

public sealed record GetFgsInventoryTransactionByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventoryTransactionDetailDto>>;
