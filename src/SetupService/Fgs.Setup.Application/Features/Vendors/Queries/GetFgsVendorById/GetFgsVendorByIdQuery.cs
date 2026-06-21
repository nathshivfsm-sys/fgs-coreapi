using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vendors.Queries.GetFgsVendorById;

public sealed record GetFgsVendorByIdQuery(long Id)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
