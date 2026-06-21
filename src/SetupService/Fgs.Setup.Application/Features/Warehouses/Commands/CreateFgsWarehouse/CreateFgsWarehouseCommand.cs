using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Commands.CreateFgsWarehouse;

public sealed record CreateFgsWarehouseCommand(FgsWarehouseCreateDto Dto)
    : IRequest<ApiResponse<FgsWarehouseDetailDto>>;
