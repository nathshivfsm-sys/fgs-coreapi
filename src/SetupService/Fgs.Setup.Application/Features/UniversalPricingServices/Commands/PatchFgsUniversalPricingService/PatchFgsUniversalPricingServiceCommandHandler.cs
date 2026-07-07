using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Commands.PatchFgsUniversalPricingService;

public sealed class PatchFgsUniversalPricingServiceCommandHandler(
    IFgsUniversalPricingServiceWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsUniversalPricingServiceCommandHandler> logger)
    : IRequestHandler<PatchFgsUniversalPricingServiceCommand, ApiResponse<FgsUniversalPricingServiceDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalPricingServiceDetailDto>> Handle(
        PatchFgsUniversalPricingServiceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd universal pricing service {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalpricingservice"),
                cancellationToken);
        return ApiResponse<FgsUniversalPricingServiceDetailDto>.Ok(result);
    }
}
