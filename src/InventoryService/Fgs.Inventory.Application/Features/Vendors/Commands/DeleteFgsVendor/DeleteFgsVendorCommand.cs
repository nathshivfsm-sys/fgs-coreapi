using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Commands.DeleteFgsVendor;

public sealed record DeleteFgsVendorCommand(long Id)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
