using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.PatchFgsSetupPricingMatrixOther;

public sealed class PatchFgsSetupPricingMatrixOtherCommandHandler(
    IFgsSetupPricingMatrixOtherWriteService writeService, ICacheService cache, ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSetupPricingMatrixOtherCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupPricingMatrixOtherCommand, ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>> Handle(PatchFgsSetupPricingMatrixOtherCommand request, CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched Pricing Matrix Other {Id}", result.Id);
        var scope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(CacheKeys.EntityPrefix(scope.TenantId, scope.CompanyId, "pricingmatrixother"), cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixOtherDetailDto>.Ok(result);
    }
}
