using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.UpdateFgsSetupPricingMatrix;

public sealed record UpdateFgsSetupPricingMatrixCommand(long Id, FgsSetupPricingMatrixUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupPricingMatrixDetailDto>>;
