using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Commands.UpdateFgsVendor;

public sealed record UpdateFgsVendorCommand(long Id, FgsVendorUpdateDto Dto)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
