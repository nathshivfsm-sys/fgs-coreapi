using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.PatchFgsUniversalMatrixOneTimeFee;

public sealed record PatchFgsUniversalMatrixOneTimeFeeCommand(long Id, FgsUniversalMatrixOneTimeFeePatchDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>>;
