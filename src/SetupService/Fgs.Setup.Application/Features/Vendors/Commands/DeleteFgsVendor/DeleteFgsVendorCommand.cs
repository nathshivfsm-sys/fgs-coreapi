using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vendors.Commands.DeleteFgsVendor;

public sealed record DeleteFgsVendorCommand(long Id)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
