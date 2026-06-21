using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Commands.UpdateFgsWarehouse;

public sealed record UpdateFgsWarehouseCommand(long Id, FgsWarehouseUpdateDto Dto)
    : IRequest<ApiResponse<FgsWarehouseDetailDto>>;
