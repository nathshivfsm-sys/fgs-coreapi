using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.UpdateFgsUniversalMatrixSizeTier;

public sealed record UpdateFgsUniversalMatrixSizeTierCommand(long Id, FgsUniversalMatrixSizeTierUpdateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixSizeTierDetailDto>>;
