using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.UpdateFgsSetupPricingMatrixLabor;

public sealed class UpdateFgsSetupPricingMatrixLaborCommandHandler(
    IFgsSetupPricingMatrixLaborWriteService writeService, ICacheService cache, ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsSetupPricingMatrixLaborCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupPricingMatrixLaborCommand, ApiResponse<FgsSetupPricingMatrixLaborDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixLaborDetailDto>> Handle(UpdateFgsSetupPricingMatrixLaborCommand request, CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated Pricing Matrix Labor {Id}", result.Id);
        var scope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(CacheKeys.EntityPrefix(scope.TenantId, scope.CompanyId, "pricingmatrixlabor"), cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixLaborDetailDto>.Ok(result);
    }
}
