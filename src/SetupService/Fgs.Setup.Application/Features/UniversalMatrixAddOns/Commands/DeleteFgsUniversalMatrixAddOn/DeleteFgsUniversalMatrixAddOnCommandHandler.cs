using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.DeleteFgsUniversalMatrixAddOn;

public sealed class DeleteFgsUniversalMatrixAddOnCommandHandler(
    IFgsUniversalMatrixAddOnWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsUniversalMatrixAddOnCommandHandler> logger)
    : IRequestHandler<DeleteFgsUniversalMatrixAddOnCommand, ApiResponse<FgsUniversalMatrixAddOnDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixAddOnDetailDto>> Handle(
        DeleteFgsUniversalMatrixAddOnCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted universal matrix add-on {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixaddon"),
            cancellationToken);
        return ApiResponse<FgsUniversalMatrixAddOnDetailDto>.Ok(result);
    }
}
