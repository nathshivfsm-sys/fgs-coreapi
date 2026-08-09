using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.UpdateFgsUniversalMatrixAddOn;

public sealed class UpdateFgsUniversalMatrixAddOnCommandHandler(
    IFgsUniversalMatrixAddOnWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsUniversalMatrixAddOnCommandHandler> logger)
    : IRequestHandler<UpdateFgsUniversalMatrixAddOnCommand, ApiResponse<FgsUniversalMatrixAddOnDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixAddOnDetailDto>> Handle(
        UpdateFgsUniversalMatrixAddOnCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated universal matrix add-on {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixaddon"),
            cancellationToken);
        return ApiResponse<FgsUniversalMatrixAddOnDetailDto>.Ok(result);
    }
}
