using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Commands.CreateFgsUniversalPricingService;

public sealed class CreateFgsUniversalPricingServiceCommandHandler(
    IFgsUniversalPricingServiceWriteService writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsUniversalPricingServiceCommandHandler> logger)
    : IRequestHandler<CreateFgsUniversalPricingServiceCommand, ApiResponse<FgsUniversalPricingServiceDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalPricingServiceDetailDto>> Handle(
        CreateFgsUniversalPricingServiceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created universal pricing service {Id} with code {UniversalPricingServiceCode}", result.Id, result.UniversalPricingServiceCode);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalpricingservice"),
                cancellationToken);
        return ApiResponse<FgsUniversalPricingServiceDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
