using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.PatchFgsUniversalMatrixItem;

public sealed class PatchFgsUniversalMatrixItemCommandHandler(
    IFgsUniversalMatrixItemWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsUniversalMatrixItemCommandHandler> logger)
    : IRequestHandler<PatchFgsUniversalMatrixItemCommand, ApiResponse<FgsUniversalMatrixItemDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixItemDetailDto>> Handle(
        PatchFgsUniversalMatrixItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd universal matrix item {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixitem"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixItemDetailDto>.Ok(result);
    }
}
