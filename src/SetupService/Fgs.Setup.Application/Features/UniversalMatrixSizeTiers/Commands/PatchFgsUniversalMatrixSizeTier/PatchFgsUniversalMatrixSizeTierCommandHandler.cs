using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.PatchFgsUniversalMatrixSizeTier;

public sealed class PatchFgsUniversalMatrixSizeTierCommandHandler(
    IFgsUniversalMatrixSizeTierWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsUniversalMatrixSizeTierCommandHandler> logger)
    : IRequestHandler<PatchFgsUniversalMatrixSizeTierCommand, ApiResponse<FgsUniversalMatrixSizeTierDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixSizeTierDetailDto>> Handle(
        PatchFgsUniversalMatrixSizeTierCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd universal matrix size tier {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixsizetier"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixSizeTierDetailDto>.Ok(result);
    }
}
