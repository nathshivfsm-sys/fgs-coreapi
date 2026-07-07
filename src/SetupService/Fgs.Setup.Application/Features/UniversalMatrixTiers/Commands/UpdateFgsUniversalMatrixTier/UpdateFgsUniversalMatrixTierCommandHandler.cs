using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.UpdateFgsUniversalMatrixTier;

public sealed class UpdateFgsUniversalMatrixTierCommandHandler(
    IFgsUniversalMatrixTierWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsUniversalMatrixTierCommandHandler> logger)
    : IRequestHandler<UpdateFgsUniversalMatrixTierCommand, ApiResponse<FgsUniversalMatrixTierDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixTierDetailDto>> Handle(
        UpdateFgsUniversalMatrixTierCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated universal matrix tier {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixtier"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixTierDetailDto>.Ok(result);
    }
}
