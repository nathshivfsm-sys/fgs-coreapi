using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySerials.Commands.UpdateFgsInventorySerial;

public sealed record UpdateFgsInventorySerialCommand(long Id, FgsInventorySerialUpdateDto Dto)
    : IRequest<ApiResponse<FgsInventorySerialDetailDto>>;
