using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.GetFgsUniversalMatrixFrequencyDiscountById;

public sealed class GetFgsUniversalMatrixFrequencyDiscountByIdQueryHandler(
    IFgsUniversalMatrixFrequencyDiscountReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsUniversalMatrixFrequencyDiscountByIdQuery, ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>> Handle(
        GetFgsUniversalMatrixFrequencyDiscountByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalmatrixfrequencydiscount",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsUniversalMatrixFrequencyDiscountDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>.Fail(
                [$"Universal Matrix Frequency Discount '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>.Ok(result);
    }
}
