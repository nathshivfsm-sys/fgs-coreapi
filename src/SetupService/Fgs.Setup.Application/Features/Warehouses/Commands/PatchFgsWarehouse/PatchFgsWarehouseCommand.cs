using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Commands.PatchFgsWarehouse;

public sealed record PatchFgsWarehouseCommand(long Id, FgsWarehousePatchDto Dto)
    : IRequest<ApiResponse<FgsWarehouseDetailDto>>;
