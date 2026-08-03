using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.PatchFgsUniversalMatrixTier;

public sealed record PatchFgsUniversalMatrixTierCommand(long Id, FgsUniversalMatrixTierPatchDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixTierDetailDto>>;
