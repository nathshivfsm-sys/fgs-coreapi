using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.DeleteFgsUniversalMatrixItem;

public sealed class DeleteFgsUniversalMatrixItemCommandHandler(
    IFgsUniversalMatrixItemWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsUniversalMatrixItemCommandHandler> logger)
    : IRequestHandler<DeleteFgsUniversalMatrixItemCommand, ApiResponse<FgsUniversalMatrixItemDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixItemDetailDto>> Handle(
        DeleteFgsUniversalMatrixItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted universal matrix item {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixitem"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixItemDetailDto>.Ok(result);
    }
}
