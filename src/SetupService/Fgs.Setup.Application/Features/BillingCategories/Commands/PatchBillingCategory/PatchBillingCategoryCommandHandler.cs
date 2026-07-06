using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.PatchBillingCategory;

public sealed class PatchBillingCategoryCommandHandler(
    IBillingCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchBillingCategoryCommandHandler> logger)
    : IRequestHandler<PatchBillingCategoryCommand, ApiResponse<BillingCategoryDetailDto>>
{
    public async Task<ApiResponse<BillingCategoryDetailDto>> Handle(
        PatchBillingCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd billing category {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "billingcategory"),
                cancellationToken);
        return ApiResponse<BillingCategoryDetailDto>.Ok(result);
    }
}
