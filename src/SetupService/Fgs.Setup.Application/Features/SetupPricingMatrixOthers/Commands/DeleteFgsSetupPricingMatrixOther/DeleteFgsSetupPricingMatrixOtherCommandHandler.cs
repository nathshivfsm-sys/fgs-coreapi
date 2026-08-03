using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.DeleteFgsSetupPricingMatrixOther;

public sealed class DeleteFgsSetupPricingMatrixOtherCommandHandler(
    IFgsSetupPricingMatrixOtherWriteService writeService, ICacheService cache, ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsSetupPricingMatrixOtherCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupPricingMatrixOtherCommand, ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>> Handle(DeleteFgsSetupPricingMatrixOtherCommand request, CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted Pricing Matrix Other {Id}", result.Id);
        var scope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(CacheKeys.EntityPrefix(scope.TenantId, scope.CompanyId, "pricingmatrixother"), cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixOtherDetailDto>.Ok(result);
    }
}
