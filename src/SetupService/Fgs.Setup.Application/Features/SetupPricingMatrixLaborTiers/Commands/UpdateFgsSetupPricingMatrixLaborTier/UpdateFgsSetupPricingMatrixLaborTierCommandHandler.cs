using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Commands.UpdateFgsSetupPricingMatrixLaborTier;

public sealed class UpdateFgsSetupPricingMatrixLaborTierCommandHandler(
    IFgsSetupPricingMatrixLaborTierWriteService writeService, ICacheService cache, ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsSetupPricingMatrixLaborTierCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupPricingMatrixLaborTierCommand, ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>> Handle(UpdateFgsSetupPricingMatrixLaborTierCommand request, CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated Pricing Matrix Labor Tier {Id}", result.Id);
        var scope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(CacheKeys.EntityPrefix(scope.TenantId, scope.CompanyId, "pricingmatrixlabortier"), cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>.Ok(result);
    }
}
