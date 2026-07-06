using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.DeleteBillingCategory;

public sealed class DeleteBillingCategoryCommandHandler(
    IBillingCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteBillingCategoryCommandHandler> logger)
    : IRequestHandler<DeleteBillingCategoryCommand, ApiResponse<BillingCategoryDetailDto>>
{
    public async Task<ApiResponse<BillingCategoryDetailDto>> Handle(
        DeleteBillingCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted billing category {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "billingcategory"),
                cancellationToken);
        return ApiResponse<BillingCategoryDetailDto>.Ok(result);
    }
}
