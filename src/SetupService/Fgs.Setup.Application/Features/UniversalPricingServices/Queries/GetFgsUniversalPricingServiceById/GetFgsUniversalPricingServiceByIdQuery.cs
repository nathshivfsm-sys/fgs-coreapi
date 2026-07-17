using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Queries.GetFgsUniversalPricingServiceById;

public sealed record GetFgsUniversalPricingServiceByIdQuery(long Id)
    : IRequest<ApiResponse<FgsUniversalPricingServiceDetailDto>>;
