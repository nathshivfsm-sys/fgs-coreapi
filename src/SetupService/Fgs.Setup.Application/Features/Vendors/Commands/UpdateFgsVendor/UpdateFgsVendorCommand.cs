using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vendors.Commands.UpdateFgsVendor;

public sealed record UpdateFgsVendorCommand(long Id, FgsVendorUpdateDto Dto)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
