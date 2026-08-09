using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySerials.Queries.GetFgsInventorySerialById;

public sealed record GetFgsInventorySerialByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventorySerialDetailDto>>;
