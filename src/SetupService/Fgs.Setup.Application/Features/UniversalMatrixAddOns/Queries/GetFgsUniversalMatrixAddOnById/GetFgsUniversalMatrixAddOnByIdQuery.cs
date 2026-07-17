using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.GetFgsUniversalMatrixAddOnById;

public sealed record GetFgsUniversalMatrixAddOnByIdQuery(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixAddOnDetailDto>>;
