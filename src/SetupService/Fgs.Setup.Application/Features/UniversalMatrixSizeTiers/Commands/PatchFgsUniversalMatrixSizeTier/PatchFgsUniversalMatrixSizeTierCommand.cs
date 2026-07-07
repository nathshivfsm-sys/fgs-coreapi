using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.PatchFgsUniversalMatrixSizeTier;

public sealed record PatchFgsUniversalMatrixSizeTierCommand(long Id, FgsUniversalMatrixSizeTierPatchDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixSizeTierDetailDto>>;
