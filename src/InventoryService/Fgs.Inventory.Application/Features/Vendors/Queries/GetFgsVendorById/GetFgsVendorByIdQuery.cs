using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Queries.GetFgsVendorById;

public sealed record GetFgsVendorByIdQuery(long Id)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
