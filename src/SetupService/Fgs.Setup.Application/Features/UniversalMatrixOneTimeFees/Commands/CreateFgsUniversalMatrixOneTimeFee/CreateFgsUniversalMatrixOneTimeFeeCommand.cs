using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.CreateFgsUniversalMatrixOneTimeFee;

public sealed record CreateFgsUniversalMatrixOneTimeFeeCommand(FgsUniversalMatrixOneTimeFeeCreateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>>;
