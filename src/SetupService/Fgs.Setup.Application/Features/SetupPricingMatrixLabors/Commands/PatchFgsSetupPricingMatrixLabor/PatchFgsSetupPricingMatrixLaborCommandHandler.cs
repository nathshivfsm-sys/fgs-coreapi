using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.PatchFgsSetupPricingMatrixLabor;

public sealed class PatchFgsSetupPricingMatrixLaborCommandHandler(
    IFgsSetupPricingMatrixLaborWriteService writeService, ICacheService cache, ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSetupPricingMatrixLaborCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupPricingMatrixLaborCommand, ApiResponse<FgsSetupPricingMatrixLaborDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixLaborDetailDto>> Handle(PatchFgsSetupPricingMatrixLaborCommand request, CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched Pricing Matrix Labor {Id}", result.Id);
        var scope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(CacheKeys.EntityPrefix(scope.TenantId, scope.CompanyId, "pricingmatrixlabor"), cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixLaborDetailDto>.Ok(result);
    }
}
