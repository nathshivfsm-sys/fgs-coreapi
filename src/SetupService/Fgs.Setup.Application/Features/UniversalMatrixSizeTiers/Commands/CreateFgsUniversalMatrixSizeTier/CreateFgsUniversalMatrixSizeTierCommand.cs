using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.CreateFgsUniversalMatrixSizeTier;

public sealed record CreateFgsUniversalMatrixSizeTierCommand(FgsUniversalMatrixSizeTierCreateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixSizeTierDetailDto>>;
