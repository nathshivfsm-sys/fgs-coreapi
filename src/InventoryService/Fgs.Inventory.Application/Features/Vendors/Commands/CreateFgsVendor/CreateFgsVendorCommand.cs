using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Commands.CreateFgsVendor;

public sealed record CreateFgsVendorCommand(FgsVendorCreateDto Dto)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
