using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySerials.Commands.CreateFgsInventorySerial;

public sealed record CreateFgsInventorySerialCommand(FgsInventorySerialCreateDto Dto)
    : IRequest<ApiResponse<FgsInventorySerialDetailDto>>;
