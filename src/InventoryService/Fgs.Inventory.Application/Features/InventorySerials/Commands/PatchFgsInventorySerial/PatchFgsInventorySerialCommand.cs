using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySerials.Commands.PatchFgsInventorySerial;

public sealed record PatchFgsInventorySerialCommand(long Id, FgsInventorySerialPatchDto Dto)
    : IRequest<ApiResponse<FgsInventorySerialDetailDto>>;
