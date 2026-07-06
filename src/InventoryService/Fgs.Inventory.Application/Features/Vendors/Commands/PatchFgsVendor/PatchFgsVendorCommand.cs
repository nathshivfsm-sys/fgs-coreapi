using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Commands.PatchFgsVendor;

public sealed record PatchFgsVendorCommand(long Id, FgsVendorPatchDto Dto)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
