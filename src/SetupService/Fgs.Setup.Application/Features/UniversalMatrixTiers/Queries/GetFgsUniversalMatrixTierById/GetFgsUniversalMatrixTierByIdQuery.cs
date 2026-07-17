using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Queries.GetFgsUniversalMatrixTierById;

public sealed record GetFgsUniversalMatrixTierByIdQuery(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixTierDetailDto>>;
