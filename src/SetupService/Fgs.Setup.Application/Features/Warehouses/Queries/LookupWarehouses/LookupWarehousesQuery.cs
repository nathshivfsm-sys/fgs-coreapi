using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Queries.LookupWarehouses;

public sealed record LookupWarehousesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsWarehouseLookupDto>>>;
