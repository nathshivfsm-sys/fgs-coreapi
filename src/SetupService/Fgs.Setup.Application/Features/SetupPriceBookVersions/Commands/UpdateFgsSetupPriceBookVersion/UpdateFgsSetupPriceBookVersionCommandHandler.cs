using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.UpdateFgsSetupPriceBookVersion;

public sealed class UpdateFgsSetupPriceBookVersionCommandHandler(
    IFgsSetupPriceBookVersionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsSetupPriceBookVersionCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupPriceBookVersionCommand, ApiResponse<FgsSetupPriceBookVersionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPriceBookVersionDetailDto>> Handle(
        UpdateFgsSetupPriceBookVersionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation(
            "Updated price book version {Id} ({Version})",
            result.Id,
            result.Version);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricebookversion"),
            cancellationToken);
        return ApiResponse<FgsSetupPriceBookVersionDetailDto>.Ok(result);
    }
}
