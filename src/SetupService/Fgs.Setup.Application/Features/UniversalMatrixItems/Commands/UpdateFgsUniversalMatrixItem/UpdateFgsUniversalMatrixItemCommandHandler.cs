using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.UpdateFgsUniversalMatrixItem;

public sealed class UpdateFgsUniversalMatrixItemCommandHandler(
    IFgsUniversalMatrixItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsUniversalMatrixItemCommandHandler> logger)
    : IRequestHandler<UpdateFgsUniversalMatrixItemCommand, ApiResponse<FgsUniversalMatrixItemDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixItemDetailDto>> Handle(
        UpdateFgsUniversalMatrixItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated universal matrix item {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixitem"),
            cancellationToken);
        return ApiResponse<FgsUniversalMatrixItemDetailDto>.Ok(result);
    }
}
