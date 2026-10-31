using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.PatchFgsSetupPriceBookVersion;

public sealed class PatchFgsSetupPriceBookVersionCommandHandler(
    IFgsSetupPriceBookVersionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSetupPriceBookVersionCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupPriceBookVersionCommand, ApiResponse<FgsSetupPriceBookVersionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPriceBookVersionDetailDto>> Handle(
        PatchFgsSetupPriceBookVersionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched price book version {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricebookversion"),
            cancellationToken);
        return ApiResponse<FgsSetupPriceBookVersionDetailDto>.Ok(result);
    }
}
