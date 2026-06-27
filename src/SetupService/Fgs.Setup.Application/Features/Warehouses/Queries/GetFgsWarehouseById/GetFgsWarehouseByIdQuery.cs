using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Queries.GetFgsWarehouseById;

public sealed record GetFgsWarehouseByIdQuery(long Id)
    : IRequest<ApiResponse<FgsWarehouseDetailDto>>;
