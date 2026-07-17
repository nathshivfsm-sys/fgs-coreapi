using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.DeleteFgsUniversalMatrixTier;

public sealed class DeleteFgsUniversalMatrixTierCommandHandler(
    IFgsUniversalMatrixTierWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsUniversalMatrixTierCommandHandler> logger)
    : IRequestHandler<DeleteFgsUniversalMatrixTierCommand, ApiResponse<FgsUniversalMatrixTierDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixTierDetailDto>> Handle(
        DeleteFgsUniversalMatrixTierCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted universal matrix tier {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixtier"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixTierDetailDto>.Ok(result);
    }
}
