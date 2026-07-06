using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.PatchFgsSetupLaborRateType;

public sealed class PatchFgsSetupLaborRateTypeCommandHandler(
    IFgsSetupLaborRateTypeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSetupLaborRateTypeCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupLaborRateTypeCommand, ApiResponse<FgsSetupLaborRateTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupLaborRateTypeDetailDto>> Handle(
        PatchFgsSetupLaborRateTypeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd labor rate type {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "laborratetype"),
                cancellationToken);
        return ApiResponse<FgsSetupLaborRateTypeDetailDto>.Ok(result);
    }
}
