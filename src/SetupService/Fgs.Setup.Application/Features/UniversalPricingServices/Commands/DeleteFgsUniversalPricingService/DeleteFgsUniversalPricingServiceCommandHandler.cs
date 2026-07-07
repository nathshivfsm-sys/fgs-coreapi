using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Commands.DeleteFgsUniversalPricingService;

public sealed class DeleteFgsUniversalPricingServiceCommandHandler(
    IFgsUniversalPricingServiceWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsUniversalPricingServiceCommandHandler> logger)
    : IRequestHandler<DeleteFgsUniversalPricingServiceCommand, ApiResponse<FgsUniversalPricingServiceDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalPricingServiceDetailDto>> Handle(
        DeleteFgsUniversalPricingServiceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted universal pricing service {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalpricingservice"),
                cancellationToken);
        return ApiResponse<FgsUniversalPricingServiceDetailDto>.Ok(result);
    }
}
