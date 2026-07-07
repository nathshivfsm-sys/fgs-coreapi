using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Commands.UpdateFgsUniversalPricingService;

public sealed record UpdateFgsUniversalPricingServiceCommand(long Id, FgsUniversalPricingServiceUpdateDto Dto)
    : IRequest<ApiResponse<FgsUniversalPricingServiceDetailDto>>;
