using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.UpdateFgsUniversalMatrixFrequencyDiscount;

public sealed record UpdateFgsUniversalMatrixFrequencyDiscountCommand(long Id, FgsUniversalMatrixFrequencyDiscountUpdateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>>;
