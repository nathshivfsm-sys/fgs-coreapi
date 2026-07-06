using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.DeleteFgsSalesActivityOutcome;

public sealed class DeleteFgsSalesActivityOutcomeCommandHandler(
    IFgsSalesActivityOutcomeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsSalesActivityOutcomeCommandHandler> logger)
    : IRequestHandler<DeleteFgsSalesActivityOutcomeCommand, ApiResponse<FgsSalesActivityOutcomeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityOutcomeDetailDto>> Handle(
        DeleteFgsSalesActivityOutcomeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted sales activity outcome {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "salesactivityoutcome"),
                cancellationToken);
        return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Ok(result);
    }
}
