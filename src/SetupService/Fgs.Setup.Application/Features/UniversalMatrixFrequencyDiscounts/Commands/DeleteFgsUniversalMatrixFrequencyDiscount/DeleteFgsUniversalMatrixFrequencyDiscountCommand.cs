using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.DeleteFgsUniversalMatrixFrequencyDiscount;

public sealed record DeleteFgsUniversalMatrixFrequencyDiscountCommand(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>>;
