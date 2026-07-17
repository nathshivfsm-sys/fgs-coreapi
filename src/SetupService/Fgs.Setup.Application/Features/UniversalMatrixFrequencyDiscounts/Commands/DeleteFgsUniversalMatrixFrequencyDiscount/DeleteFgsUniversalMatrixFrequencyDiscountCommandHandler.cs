using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.DeleteFgsUniversalMatrixFrequencyDiscount;

public sealed class DeleteFgsUniversalMatrixFrequencyDiscountCommandHandler(
    IFgsUniversalMatrixFrequencyDiscountWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsUniversalMatrixFrequencyDiscountCommandHandler> logger)
    : IRequestHandler<DeleteFgsUniversalMatrixFrequencyDiscountCommand, ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>> Handle(
        DeleteFgsUniversalMatrixFrequencyDiscountCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted universal matrix frequency discount {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixfrequencydiscount"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>.Ok(result);
    }
}
