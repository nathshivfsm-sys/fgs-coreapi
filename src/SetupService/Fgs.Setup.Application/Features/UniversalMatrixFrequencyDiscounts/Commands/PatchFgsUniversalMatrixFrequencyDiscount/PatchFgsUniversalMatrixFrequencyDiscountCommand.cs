using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.PatchFgsUniversalMatrixFrequencyDiscount;

public sealed record PatchFgsUniversalMatrixFrequencyDiscountCommand(long Id, FgsUniversalMatrixFrequencyDiscountPatchDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>>;
