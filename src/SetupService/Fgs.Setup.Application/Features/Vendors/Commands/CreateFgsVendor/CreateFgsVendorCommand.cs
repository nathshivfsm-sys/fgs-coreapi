using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vendors.Commands.CreateFgsVendor;

public sealed record CreateFgsVendorCommand(FgsVendorCreateDto Dto)
    : IRequest<ApiResponse<FgsVendorDetailDto>>;
