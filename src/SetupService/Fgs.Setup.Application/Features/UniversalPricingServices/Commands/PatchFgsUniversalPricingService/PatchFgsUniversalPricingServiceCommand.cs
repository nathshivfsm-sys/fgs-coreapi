using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Commands.PatchFgsUniversalPricingService;

public sealed record PatchFgsUniversalPricingServiceCommand(long Id, FgsUniversalPricingServicePatchDto Dto)
    : IRequest<ApiResponse<FgsUniversalPricingServiceDetailDto>>;
