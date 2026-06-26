using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.PatchFgsSalesActivityOutcome;

public sealed class PatchFgsSalesActivityOutcomeCommandHandler(
    IFgsSalesActivityOutcomeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSalesActivityOutcomeCommandHandler> logger)
    : IRequestHandler<PatchFgsSalesActivityOutcomeCommand, ApiResponse<FgsSalesActivityOutcomeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityOutcomeDetailDto>> Handle(
        PatchFgsSalesActivityOutcomeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd sales activity outcome {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "salesactivityoutcomes"),
                cancellationToken);
        return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Ok(result);
    }
}
