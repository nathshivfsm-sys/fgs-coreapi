using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.GetFgsUniversalMatrixFrequencyDiscountById;

public sealed record GetFgsUniversalMatrixFrequencyDiscountByIdQuery(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>>;
