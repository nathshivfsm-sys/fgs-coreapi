using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Commands.CreateFgsUniversalPricingService;

public sealed record CreateFgsUniversalPricingServiceCommand(FgsUniversalPricingServiceCreateDto Dto)
    : IRequest<ApiResponse<FgsUniversalPricingServiceDetailDto>>;
