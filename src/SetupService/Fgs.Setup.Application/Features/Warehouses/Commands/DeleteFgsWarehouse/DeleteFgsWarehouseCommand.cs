using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Commands.DeleteFgsWarehouse;

public sealed record DeleteFgsWarehouseCommand(long Id)
    : IRequest<ApiResponse<FgsWarehouseDetailDto>>;
