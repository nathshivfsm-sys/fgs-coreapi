using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.PatchFgsSetupPricingMatrix;

public sealed class PatchFgsSetupPricingMatrixCommandHandler(
    IFgsSetupPricingMatrixWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSetupPricingMatrixCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupPricingMatrixCommand, ApiResponse<FgsSetupPricingMatrixDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixDetailDto>> Handle(
        PatchFgsSetupPricingMatrixCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patched pricing matrix {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricingmatrix"),
                cancellationToken);
            await cache.RemoveAsync(
                CacheKeys.Build(tenantScope.TenantId, tenantScope.CompanyId, "pricingmatrix", request.Id.ToString()),
                cancellationToken);
            return ApiResponse<FgsSetupPricingMatrixDetailDto>.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return ApiResponse<FgsSetupPricingMatrixDetailDto>.Fail(
                [$"Pricing matrix '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }
    }
}
