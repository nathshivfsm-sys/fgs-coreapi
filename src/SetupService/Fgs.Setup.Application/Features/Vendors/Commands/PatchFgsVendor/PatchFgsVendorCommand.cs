using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vendors.Commands.PatchFgsVendor;

public sealed record PatchFgsVendorCommand(long Id, FgsVendorPatchDto Dto)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
