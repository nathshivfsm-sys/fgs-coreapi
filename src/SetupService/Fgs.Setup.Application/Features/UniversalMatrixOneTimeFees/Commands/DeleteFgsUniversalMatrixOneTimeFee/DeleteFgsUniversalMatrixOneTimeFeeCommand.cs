using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.DeleteFgsUniversalMatrixOneTimeFee;

public sealed record DeleteFgsUniversalMatrixOneTimeFeeCommand(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>>;
