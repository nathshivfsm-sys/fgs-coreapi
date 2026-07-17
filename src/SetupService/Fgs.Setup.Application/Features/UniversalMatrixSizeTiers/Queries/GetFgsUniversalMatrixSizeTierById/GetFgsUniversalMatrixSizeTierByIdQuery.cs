using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.GetFgsUniversalMatrixSizeTierById;

public sealed record GetFgsUniversalMatrixSizeTierByIdQuery(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixSizeTierDetailDto>>;
