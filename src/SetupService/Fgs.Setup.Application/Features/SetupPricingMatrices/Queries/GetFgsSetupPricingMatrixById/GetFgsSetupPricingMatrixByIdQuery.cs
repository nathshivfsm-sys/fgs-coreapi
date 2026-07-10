using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Queries.GetFgsSetupPricingMatrixById;

public sealed record GetFgsSetupPricingMatrixByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupPricingMatrixDetailDto>>;
