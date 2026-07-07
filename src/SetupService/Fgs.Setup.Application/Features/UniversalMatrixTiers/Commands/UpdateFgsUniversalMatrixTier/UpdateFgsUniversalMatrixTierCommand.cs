using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.UpdateFgsUniversalMatrixTier;

public sealed record UpdateFgsUniversalMatrixTierCommand(long Id, FgsUniversalMatrixTierUpdateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixTierDetailDto>>;
