using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.UpdateFgsUniversalMatrixSizeTier;

public sealed class UpdateFgsUniversalMatrixSizeTierCommandHandler(
    IFgsUniversalMatrixSizeTierWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsUniversalMatrixSizeTierCommandHandler> logger)
    : IRequestHandler<UpdateFgsUniversalMatrixSizeTierCommand, ApiResponse<FgsUniversalMatrixSizeTierDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixSizeTierDetailDto>> Handle(
        UpdateFgsUniversalMatrixSizeTierCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated universal matrix size tier {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixsizetier"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixSizeTierDetailDto>.Ok(result);
    }
}
