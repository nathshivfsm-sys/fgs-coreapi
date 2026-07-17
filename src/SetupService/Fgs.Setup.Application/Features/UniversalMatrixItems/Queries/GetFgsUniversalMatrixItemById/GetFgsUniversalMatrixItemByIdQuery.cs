using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Queries.GetFgsUniversalMatrixItemById;

public sealed record GetFgsUniversalMatrixItemByIdQuery(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixItemDetailDto>>;
