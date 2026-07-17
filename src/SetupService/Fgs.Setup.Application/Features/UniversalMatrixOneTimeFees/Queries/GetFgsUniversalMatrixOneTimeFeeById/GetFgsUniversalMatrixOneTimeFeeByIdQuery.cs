using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Queries.GetFgsUniversalMatrixOneTimeFeeById;

public sealed record GetFgsUniversalMatrixOneTimeFeeByIdQuery(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>>;
