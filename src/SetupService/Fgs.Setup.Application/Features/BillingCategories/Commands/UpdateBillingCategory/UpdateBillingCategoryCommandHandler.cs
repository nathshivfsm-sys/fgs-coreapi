using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.UpdateBillingCategory;

public sealed class UpdateBillingCategoryCommandHandler(
    IBillingCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateBillingCategoryCommandHandler> logger)
    : IRequestHandler<UpdateBillingCategoryCommand, ApiResponse<BillingCategoryDetailDto>>
{
    public async Task<ApiResponse<BillingCategoryDetailDto>> Handle(
        UpdateBillingCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated billing category {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "billingcategory"),
                cancellationToken);
        return ApiResponse<BillingCategoryDetailDto>.Ok(result);
    }
}
