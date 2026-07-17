using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.DeleteFgsUniversalMatrixTier;

public sealed record DeleteFgsUniversalMatrixTierCommand(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixTierDetailDto>>;
