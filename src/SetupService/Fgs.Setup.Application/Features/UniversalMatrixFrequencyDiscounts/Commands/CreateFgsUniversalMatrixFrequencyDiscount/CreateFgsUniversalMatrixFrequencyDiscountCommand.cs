using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.CreateFgsUniversalMatrixFrequencyDiscount;

public sealed record CreateFgsUniversalMatrixFrequencyDiscountCommand(FgsUniversalMatrixFrequencyDiscountCreateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>>;
