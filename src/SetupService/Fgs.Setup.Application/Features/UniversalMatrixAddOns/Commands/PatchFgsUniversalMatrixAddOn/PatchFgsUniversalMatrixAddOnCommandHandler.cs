using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.PatchFgsUniversalMatrixAddOn;

public sealed class PatchFgsUniversalMatrixAddOnCommandHandler(
    IFgsUniversalMatrixAddOnWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsUniversalMatrixAddOnCommandHandler> logger)
    : IRequestHandler<PatchFgsUniversalMatrixAddOnCommand, ApiResponse<FgsUniversalMatrixAddOnDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixAddOnDetailDto>> Handle(
        PatchFgsUniversalMatrixAddOnCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd universal matrix add-on {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixaddon"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixAddOnDetailDto>.Ok(result);
    }
}
