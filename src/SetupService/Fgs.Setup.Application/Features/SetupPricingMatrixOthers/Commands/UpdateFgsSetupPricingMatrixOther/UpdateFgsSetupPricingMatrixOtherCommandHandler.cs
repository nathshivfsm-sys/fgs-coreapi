using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.UpdateFgsSetupPricingMatrixOther;

public sealed class UpdateFgsSetupPricingMatrixOtherCommandHandler(
    IFgsSetupPricingMatrixOtherWriteService writeService, ICacheService cache, ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsSetupPricingMatrixOtherCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupPricingMatrixOtherCommand, ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>> Handle(UpdateFgsSetupPricingMatrixOtherCommand request, CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated Pricing Matrix Other {Id}", result.Id);
        var scope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(CacheKeys.EntityPrefix(scope.TenantId, scope.CompanyId, "pricingmatrixother"), cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixOtherDetailDto>.Ok(result);
    }
}
