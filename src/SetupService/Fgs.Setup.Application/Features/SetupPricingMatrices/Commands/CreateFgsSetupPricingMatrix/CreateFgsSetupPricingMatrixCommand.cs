using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;
using Fgs.Contracts.Api;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.CreateFgsSetupPricingMatrix;

public sealed record CreateFgsSetupPricingMatrixCommand(FgsSetupPricingMatrixCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupPricingMatrixDetailDto>>;
