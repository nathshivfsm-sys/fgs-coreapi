using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Commands.DeleteFgsUniversalPricingService;

public sealed record DeleteFgsUniversalPricingServiceCommand(long Id)
    : IRequest<ApiResponse<FgsUniversalPricingServiceDetailDto>>;
