using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.UpdateFgsUniversalMatrixOneTimeFee;

public sealed record UpdateFgsUniversalMatrixOneTimeFeeCommand(long Id, FgsUniversalMatrixOneTimeFeeUpdateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>>;
