using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.DeleteFgsUniversalMatrixOneTimeFee;

public sealed class DeleteFgsUniversalMatrixOneTimeFeeCommandHandler(
    IFgsUniversalMatrixOneTimeFeeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsUniversalMatrixOneTimeFeeCommandHandler> logger)
    : IRequestHandler<DeleteFgsUniversalMatrixOneTimeFeeCommand, ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>> Handle(
        DeleteFgsUniversalMatrixOneTimeFeeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted universal matrix one-time fee {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixonetimefee"),
            cancellationToken);
        return ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>.Ok(result);
    }
}
