using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.UpdateFgsUniversalMatrixFrequencyDiscount;

public sealed class UpdateFgsUniversalMatrixFrequencyDiscountCommandHandler(
    IFgsUniversalMatrixFrequencyDiscountWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsUniversalMatrixFrequencyDiscountCommandHandler> logger)
    : IRequestHandler<UpdateFgsUniversalMatrixFrequencyDiscountCommand, ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>> Handle(
        UpdateFgsUniversalMatrixFrequencyDiscountCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated universal matrix frequency discount {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixfrequencydiscount"),
            cancellationToken);
        return ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>.Ok(result);
    }
}
