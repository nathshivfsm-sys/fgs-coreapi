using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.CreateFgsUniversalMatrixTier;

public sealed record CreateFgsUniversalMatrixTierCommand(FgsUniversalMatrixTierCreateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixTierDetailDto>>;
