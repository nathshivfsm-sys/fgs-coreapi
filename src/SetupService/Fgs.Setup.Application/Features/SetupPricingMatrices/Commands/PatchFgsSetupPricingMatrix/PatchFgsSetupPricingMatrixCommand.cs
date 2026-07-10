using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.PatchFgsSetupPricingMatrix;

public sealed record PatchFgsSetupPricingMatrixCommand(long Id, FgsSetupPricingMatrixPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupPricingMatrixDetailDto>>;
